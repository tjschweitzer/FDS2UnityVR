using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

//Andrew Droubay
//Dr. Anil Shende
//Roanoke College
//USFS Unity Research
// 8/2020

//PRE: This is the first file that will run for the simulation. A valid fds input
//     file must be the only file in the Unity StreamingAssets folder
//POST: Creates the terrain mesh
//      Sets accessible ncols, nrows, CHID, vertices, faces, Tstart, Tend, Tstep

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{

    public GameObject tree_prefab; //the object for trees
    protected int camera_height; //height of the camera
    protected Sprite sprite; //unused
    Mesh mesh;

    protected StreamReader terrain_file;
    public static int cellsize; //size of one terrain cell
    public static int ncols;
    public static int nrows;
    public static string CHID; //job id
    public static int Tstart; //job start time
    public static int Tend; //job end time
    public static int Tstep; //how much time increments by
    public static string file_name;
    public MeshCollider collider;

    public static List<Vector3> terrain_verts;
    public static List<int> terrain_faces;
    public GameObject configData;
    private ConfigData config_script;

    // This is the first function Unity calls
    void Awake()
    {
        Debug.Log("Starts TerrainGeneration");
      //read in the wfds input file from the streaming assets folder
      DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
      FileInfo[] files = directoryInfo.GetFiles();
      terrain_verts = new List<Vector3>();
      terrain_faces = new List<int>();

      config_script= configData.GetComponent<ConfigData>();

      file_name = config_script.fileName;
      
        //open and close the file reader to set the global variables
        using (StreamReader global_reader = new StreamReader(file_name))
       {
          SetGlobals(global_reader);
          //  Debug.Log("tstart: " + Tstart);
       }

        terrain_file = new StreamReader(file_name);
        

        //Create the terrain mesh
        mesh = new Mesh(); //Mesh to hold the terrain
        GetComponent<MeshFilter>().mesh = mesh; //Sets the mesh filter

        //set up mesh
        mesh.Clear();
        mesh.vertices = GetVerts().ToArray();
        mesh.triangles = GetFaces().ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        collider.sharedMesh = null; //<-- new!!
        collider.sharedMesh = mesh;
        Mesh m = GetComponent<MeshFilter>().mesh;
        m.bounds = new Bounds(Vector3.zero, Vector3.one * 4000);

        terrain_file.Close();

        //set the faces for the fire renderer
        terrain_faces = GetFaces();

    }

    void Update()
    {
     //   mesh.RecalculateBounds();
    }

    //PRE: StreamReader object reading a wfds input file
    //POST: Set global information for CHID, ncols, nrows, and cellsize
    void SetGlobals(StreamReader reader)
    {
        //line_idx indicates the current line that reads terrain data
        string curLine;
        string Tstep_str = null;
        string Tstart_str = null;
        string Tend_str = null;
            while ((curLine = reader.ReadLine()) != null) {
              if(curLine.Contains("CHID"))
               {
                int chid_start = curLine.IndexOf("\'");
                int chid_end = curLine.LastIndexOf("\'");
                CHID = curLine.Substring(chid_start + 1, chid_end - chid_start - 1);
               }
              if(curLine.Contains("T_END"))
               {
                int tend_start = curLine.IndexOf("=");
                int tend_end = curLine.LastIndexOf("/");
                Tend_str = curLine.Substring(tend_start + 1, tend_end - tend_start - 1).Trim();
               }
             if (curLine.Contains("T_BEGIN"))
             {
                int tstart_start = curLine.IndexOf("=");
                int tstart_end = curLine.LastIndexOf("/");
                Tstart_str = curLine.Substring(tstart_start + 1, tstart_end - tstart_start - 1).Trim();
             }
            if (curLine.Contains("DT_OUTPUT_LS"))
            {
                int tstep_start = curLine.IndexOf("=");
                int tstep_end = curLine.LastIndexOf("/");
                Tstep_str = curLine.Substring(tstep_start + 1, tstep_end - tstep_start - 1).Trim();
               // Debug.Log(Tstep_str);
            }
            if (curLine.StartsWith("&MESH")) {
                
                //parsing the mesh line to get grid size
                string[] mesh_info = curLine.Replace(" ",System.String.Empty).Replace("XB=",System.String.Empty).Replace("&MESHIJK=",System.String.Empty).Replace("/",System.String.Empty).Split(',');
                int numx = int.Parse(mesh_info[0]);
                int numy = int.Parse(mesh_info[1]);
                int numz = int.Parse(mesh_info[2]);
                int xmin = int.Parse(mesh_info[3]);
                int xmax = int.Parse(mesh_info[4]);
                int ymin = int.Parse(mesh_info[5]);
                int ymax = int.Parse(mesh_info[6]);
                cellsize = (xmax - xmin) / numx;
                ncols = xmax / cellsize;
                nrows = ymax / cellsize;

              }
            }
            if(Tstart_str == null)
            {
                Tstart = 0;
            } 
            else if(double.TryParse(Tstart_str, out double val))
            {
                Tstart = (int)val;
            } else
            {
                Debug.Log("failed Tstart");
            }

            if (Tstep_str == null)
            {
                Tstep = 1;
            }
            else if (double.TryParse(Tstep_str, out double val))
            {
                Tstep = (int)val;
            }
            else
            {
                Debug.Log("failed Tstep");
            }

        if (Tend_str == null)
        {
            Tend = 1;
        }
        else if (double.TryParse(Tend_str, out double val))
        {
            Tend = (int)val;

        }
        else
        {
            Debug.Log("failed Tend");
        }

        Debug.Log("ncols: " + ncols);
        Debug.Log("nrows: " + nrows);
        Debug.Log("cellsize: " + cellsize);
        Debug.Log("tstart: " + Tstart);
        Debug.Log("tend: " + Tend);
        Debug.Log("tstep: " + Tstep);
    }

    //PRE: Global StreamReader terrain_file must be set to a wfds input file
    //POST: List<Vector3> of the vertices to construct the terrain mesh
    List<Vector3> GetVerts()
    {
        List<Vector3> terrain_list = new List<Vector3>();
        string cur_line;
        int counter = 0;
        int max_height = 0;
        //run to the end of the file
         while((cur_line = terrain_file.ReadLine()) != null) {
           if(cur_line.Trim().StartsWith("&OBST")) {
             counter++;
             string new_line = cur_line.Replace(" ", System.String.Empty).Replace("&OBSTXB=",System.String.Empty);
             string[] vert_info = new_line.Split(',');

             int temp_col = int.Parse(vert_info[1]); //col
             int temp_row = int.Parse(vert_info[3]); //row
             int temp_z = int.Parse(vert_info[5]); //height

            Vector3 point = new Vector3(temp_row, temp_z, temp_col);

            if(temp_z > max_height)
                {
                    camera_height = temp_z;
                }
                //once the inuts get more complicated info about the SURF_ID='tree object',
                //there will be a function that can generate a tree model dynamically
                //creates three trees on any line with a surface_id of TREE
             if(vert_info[6].Contains("TREE")) {

               Vector3 pos = new Vector3(point.x + 10, point.z, -point.y + 10);
               Vector3 pos2 = new Vector3(point.x + 10, point.z, -point.y + 20);
               Vector3 pos3 = new Vector3(point.x + 20, point.z, -point.y + 10);

               var curr_tree = Instantiate(tree_prefab, pos, Quaternion.identity);
               curr_tree.name = "1tree" + vert_info[1] + vert_info[3];
               curr_tree.transform.localScale = new Vector3(2,2,2);

               var curr_tree2 = Instantiate(tree_prefab, pos2, Quaternion.identity);
               curr_tree2.name = "2tree" + vert_info[1] + vert_info[3];
               curr_tree2.transform.localScale = new Vector3(2,2,2);

               var curr_tree3 = Instantiate(tree_prefab, pos3, Quaternion.identity);
               curr_tree3.name = "3tree" + vert_info[1] + vert_info[3];
               curr_tree3.transform.localScale = new Vector3(2,2,2);
             }

             terrain_list.Add(point);
             terrain_verts.Add(point);

           }
         }

         return terrain_list;
       }

    //PRE: Global data ncols, nrows must be set
    //POST: Returns List<int> of the indices of
    //     vertices that would result from GetVerts()
    List<int> GetFaces()
    {
        List<int> faces = new List<int>();
        int vertex_idx = 0;
        while (vertex_idx < (nrows - 1) * ncols)
        { //dont run on bottom line
            int cur_row = vertex_idx / ncols;
            if (vertex_idx != (ncols - 1) + (ncols * cur_row))
            { //dont run on right col

                //face 1
                faces.Add(vertex_idx);
                faces.Add(vertex_idx + 1);
                faces.Add(vertex_idx + ncols);
                //face 2
                faces.Add(vertex_idx + ncols);
                faces.Add(vertex_idx + 1);
                faces.Add(vertex_idx + ncols + 1);
            }
            vertex_idx++;
        }
        return faces;

    }


    //PRE: A Vector2D coordinate within the bounds of the terrain
    //POST: Returns the corresponding Vector3 coordinate
   // public static Vector3 find_vert(Vector2 coord)
   // {
        //return TerrainGenerator.terrain_verts.Find(vert =>
        
            //NewInputWriter.RoundDown(coord.x) == vert.x &&
            //NewInputWriter.RoundDown(coord.y) == vert.z
       // );
   // }
    
}
