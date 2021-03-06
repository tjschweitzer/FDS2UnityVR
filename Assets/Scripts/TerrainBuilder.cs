using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Valve.VR.InteractionSystem;


public class TerrainBuilder : MonoBehaviour
{
    
    Mesh mesh;

    protected StreamReader terrain_file;
    public static float cellsize; //size of one terrain cell
    public static int ncols;
    public static int nrows;
    public Vector2[] uvs;
    public static int nstacks;
    public static string CHID; //job id
    public static int Tstart; //job start time
    public static int Tend; //job end time
    public static int Tstep; //how much time increments by
    public static string file_name;
    public MeshCollider collider;
    public GameObject groundPrefab;
    private String[] terrainLabels;
    private String[] obstacleLabels;
    public static  Dictionary<string,dynamic> meshData = new Dictionary<string, dynamic>() ;
    public static  Dictionary<string,dynamic> multiData = new Dictionary<string, dynamic>() ;
    private List<String> terrainSurfIDList = new List<String>();
    // Start is called before the first frame update
    public static List<Vector3> terrain_verts;
    public static List<int> terrain_faces;
    
    public GameObject configData;
    private ConfigData config_script;
    private bool smoothTerrain;
    private Vector3 highestPoint;

    private void Start()
    {
        
        config_script= configData.GetComponent<ConfigData>();
        bool fastFuelRun = config_script.fastFuels;
        if (fastFuelRun)
        {
            return;
            
        }

        string fdsFilename = config_script.fileName;
        smoothTerrain = config_script.smoothTerrain;
        obstacleLabels = config_script.obstacleLabels;
        terrainLabels = config_script.terrainLabels;
        //
        Debug.Log("Starts TerrainGeneration");
        //read in the wfds input file from the streaming assets folder

        terrain_verts = new List<Vector3>();
        terrain_faces = new List<int>();
        FileInfo file = new FileInfo( fdsFilename);
        
        using (StreamReader fds_reader = file.OpenText())
        {
            ParseFds(fds_reader);
            terrain_file = file.OpenText();
            file_name = file.Name;
        }


        if (smoothTerrain)
        {

            //Create the terrain mesh
            mesh = new Mesh(); //Mesh to hold the terrain
            GetComponent<MeshFilter>().mesh = mesh; //Sets the mesh filter

            //set up mesh
            mesh.Clear();
            mesh.vertices = GetVerts().ToArray();
            mesh.triangles = GetFaces().ToArray();
            //GetUvs();
            // Todo: Correct faces and verts from python code to only output top of topography
            Debug.Log($"VertSize {GetVerts().ToArray().Length}   FaceSize {GetFaces().ToArray().Length}  UVs {uvs.Length}");
            mesh.uv = uvs;
            
            
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            collider.sharedMesh = mesh;
            Mesh m = GetComponent<MeshFilter>().mesh;
            terrain_file.Close();
            //set the faces for the fire renderer
            terrain_faces = GetFaces();
            MovePlayer();
            
            
            terrain_file = file.OpenText();
           // buildTerrainCubes();
            terrain_file.Close();
        }
        else
        {
            buildTerrainCubes();
        }
        
        GameObject PlayerObject = GameObject.Find("Player"); //get the SteamVR player
        PlayerObject.transform.position = highestPoint; //move to start position

    }

    // Moves the player to the highest point of the topography
    void MovePlayer()
    {

    }

    void buildTerrainCubes()
    {
        string cur_line;
        int counter = 0;
        GameObject parentGround = new GameObject("ParentofGroubObjects");
        while ((cur_line = terrain_file.ReadLine()) != null)
        {
            if (cur_line.Trim().StartsWith("&OBST"))
            {
                string currentSurfID = cur_line.Split(new string[] {"SURF_ID='"}, StringSplitOptions.None)[1]
                    .Replace("'/", "");
               
                bool terrainFlag = terrainLabels.Any(s => currentSurfID.Contains(s));
                bool obstacleFlag = obstacleLabels.Any(s => currentSurfID.Contains(s));
                if (terrainFlag || obstacleFlag)
                {



                    counter++;
                    string new_line = cur_line.Replace(" ", System.String.Empty)
                        .Replace("&OBSTXB=", System.String.Empty);
                    string[] vert_info = new_line.Split(',');
                    int temp_col = int.Parse(vert_info[0]); //col
                    int temp_col_end = int.Parse(vert_info[1]); //col
                    int temp_row = int.Parse(vert_info[2]); //row
                    int temp_row_end = int.Parse(vert_info[3]); //row
                    int temp_z = int.Parse(vert_info[5])+3; //height
                    int temp_z_bottom = int.Parse(vert_info[4]); //height


                    float colSize = Math.Abs(temp_col_end - temp_col);
                    float rowSize = Math.Abs(temp_row_end - temp_row);
                    float zSize = Math.Abs(temp_z - temp_z_bottom);

                    GameObject ground = Instantiate(groundPrefab, new Vector3((float) (temp_col + (colSize / 2)),
                            (float) (temp_z - (zSize / 2)), (float) (temp_row + (rowSize / 2))),
                        Quaternion.identity);

                    ground.transform.localScale =
                        new Vector3(colSize, zSize, rowSize);

                    if (obstacleFlag)
                    {
                        ground.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);

                    }
                    ground.transform.SetParent(parentGround.transform);
                    ground.isStatic = true;


                }
            }

        }
    }

    void ParseFds(StreamReader fdsReader)
    {
        //line_idx indicates the current line that reads terrain data
        string curLine;
        string Tstep_str = null;
        string Tstart_str = null;
        string Tend_str = null;
        

        
            while ((curLine = fdsReader.ReadLine()) != null) {
                if (!curLine.StartsWith("&")) { continue; }
                
                
                // Allows for multi line objects in fds file
                while (!curLine.Contains("/"))
                {
                    curLine += fdsReader.ReadLine();
                }
                

                if(curLine.Contains("CHID"))
               {
                    string chid_start = curLine.Split(new string[] {"CHID="}, StringSplitOptions.None)[1];
                    string chid_end = chid_start.Split(',')[0];
                    CHID = chid_end.Replace('\'', ' ').Trim();
                    Debug.Log("CHID: "+CHID);
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
            if (curLine.Contains("DT_OUTPUT"))
            {
                int tstep_start = curLine.IndexOf("=");
                int tstep_end = curLine.LastIndexOf("/");
                Tstep_str = curLine.Substring(tstep_start + 1, tstep_end - tstep_start - 1).Trim();
               // Debug.Log(Tstep_str);
            }


            if (curLine.StartsWith("&MULT"))
            {
                Debug.Log($"Multi Line {curLine}");

                var multID = curLine.Split(new string[] {"ID='"}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split('\'')[0];
                var iUpper = curLine.Split(new string[] {"I_UPPER="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                var jUpper = curLine.Split(new string[] {"J_UPPER="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                var kUpper = curLine.Split(new string[] {"K_UPPER="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                var dx = curLine.Split(new string[] {"DX="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                var dy = curLine.Split(new string[] {"DY="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                var dz = curLine.Split(new string[] {"DZ="}, StringSplitOptions.None)[1].Replace(",", " ")
                    .Split( ' ')[0];
                multiData[multID] = new Dictionary<String, float>();

                multiData[multID]["I_UPPER"] = float.Parse(iUpper);
                multiData[multID]["J_UPPER"]= float.Parse(jUpper);
                multiData[multID]["K_UPPER"]=float.Parse(kUpper);
                multiData[multID]["DX"]= float.Parse(dx);
                multiData[multID]["DY"]= float.Parse(dy);
                multiData[multID]["DZ"] = float.Parse(dz);
                
                
                Debug.Log($"Multi Line {curLine}  I_UPPER{float.Parse(iUpper)} J_UPPER {float.Parse(jUpper)} " +
                          $"K_UPPER {float.Parse(kUpper)} DX {float.Parse(dx)} DY{float.Parse(dy)} DZ  {float.Parse(dz)} MULTID {multID}");
            }
            
            if (curLine.Contains("&MESH")) {
                Debug.Log("Mesh Loaded");
                //Todo: change cell size incase voxels are not equal
                //parsing the mesh line to get grid size
                string[] mesh_info = curLine.Replace("XB=",System.String.Empty).Replace("&MESH",System.String.Empty).Replace("IJK=",String.Empty).Replace("/",System.String.Empty).Split(new char[] {',',' '},StringSplitOptions.RemoveEmptyEntries);
                Debug.Log(string.Join(", ",mesh_info));
                Debug.Log(mesh_info.Length);
                Debug.Log(mesh_info[0]);
                
                //
                float numx = float.Parse(mesh_info[0]);   
                float numy = float.Parse(mesh_info[1]);
                float numz = float.Parse(mesh_info[2]);
                float xmin = float.Parse(mesh_info[3]);
                float xmax = float.Parse(mesh_info[4]);
                float ymin = float.Parse(mesh_info[5]);
                float ymax = float.Parse(mesh_info[6]);
                float zmin = float.Parse(mesh_info[7]);
                float zmax = float.Parse(mesh_info[8]);

                if (curLine.Contains("MULT_ID="))
                {
                    var multId = curLine.Split(new string[] {"MULT_ID='"}, StringSplitOptions.None)[1].Replace(",", " ")
                        .Split( ' ')[0].Replace("'",String.Empty);
                    meshData["multID"] = multId;
                }
                else
                {
                    meshData["multID"] = String.Empty;
                }

                Debug.Log($"num xyz {numx}  {numy} {numz}");
                Debug.Log($"min xyz {xmin}  {ymin} {zmin}");
                Debug.Log($"max xyz {xmax}  {ymax} {zmax}");
                meshData["I"] = numx;
                meshData["J"] = numy;
                meshData["K"] = numz;
                if (xmin<xmax)
                {
                    meshData["xMin"] = xmin;
                    meshData["xMax"] = xmax;

                }
                else
                {
                    meshData["xMin"] = xmax;
                    meshData["xMax"] = xmin;

                }

                if (ymin<ymax)
                {
                 
                    meshData["yMin"] = ymin;
                    meshData["yMax"] = ymax;

                }
                else
                {

                    meshData["yMin"] = ymax;
                    meshData["yMax"] = ymin;

                }
                
                float xcellsize = (xmax - xmin)/numx ;
                float ycellsize = (ymax - ymin)/numy ;
                float zcellsize = (zmax - zmin)/numz ;
                
                meshData["zMin"] = zmin;
                meshData["zMax"] = zmax; 

                meshData["xSize"]  = (xmax - xmin)/numx ;
                meshData["ySize"] = (ymax - ymin)/numy ;
                meshData["zSize"] = (zmax - zmin)/numz ;
                ncols =(int) ((xmax-xmin) / xcellsize);
                nrows = (int)((ymax-ymin) / ycellsize);
                nstacks = (int)((zmax - zmin) / zcellsize);
                cellsize = xcellsize ;
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

        // calculate out multimesh true mesh size

        Debug.Log($"Mult ID {meshData["multID"]}");
        if (meshData["multID"] != String.Empty)
        {
            
            var meshID = multiData["multID"];
            var multMeshData = multiData[meshID];

            if (multMeshData["I_UPPER"]!=0 && multMeshData["DX"]>0)
            {
                meshData["xmax"] = meshID["xmin"] + (multMeshData["I_UPPER"]  + 1 ) * multMeshData["DX"];
            }else
            {
                meshData["xmin"] = meshID["xmax"] + (multMeshData["I_UPPER"]  + 1 ) * multMeshData["DX"];
            }
            
            if (multMeshData["J_UPPER"]!=0 && multMeshData["DY"]>0)
            {
                meshData["ymax"] = meshID["ymin"] + (multMeshData["J_UPPER"]  + 1 ) * multMeshData["DY"];
            }else
            {
                meshData["ymin"] = meshID["ymax"] + (multMeshData["J_UPPER"]  + 1 ) * multMeshData["DY"];
            }
            
            if (multMeshData["I_UPPER"]!=0 && multMeshData["DX"]>0)
            {
                meshData["zmax"] = meshID["zmin"] + (multMeshData["K_UPPER"]  + 1 ) * multMeshData["DZ"];
            }else
            {
                meshData["zmin"] = meshID["zmax"] + (multMeshData["K_UPPER"]  + 1 ) * multMeshData["DZ"];
            }

            
        }
        
        
        Debug.Log("ncols: " + ncols);
        Debug.Log("nrows: " + nrows);
        Debug.Log("cellsize: " + cellsize);
        Debug.Log("tstart: " + Tstart);
        Debug.Log("tend: " + Tend);
        Debug.Log("tstep: " + Tstep);
    }


    public int vertscounter;
    List<Vector3> GetVerts()
    {
        List<Vector3> terrain_list = new List<Vector3>();
        int counter = 0;
        
        // list of all verts loaded in from custom JSON
        var verts = config_script.verts;

        
        foreach (var vert in verts)
        {
            float x =(float) vert[0];
            float z =(float) vert[1];
            float y =(float) vert[2];
            if(x==0 && z ==0) Debug.Log($"X {x}  Y {y}  Z {z}  Counter {counter}");
            Vector3 point = new Vector3(x, y, z);
            
            checkHighestPoint(point);
            terrain_list.Add(point);
            counter++;
        }
        
        uvs = new Vector2[counter];

        for (int i = 0; i < counter; i++)
        {
            uvs[i] = new Vector2(terrain_list[i].x, terrain_list[i].z);
        }
        
        vertscounter = counter;
        return terrain_list;
       }

    void checkHighestPoint(Vector3 newPoint)
    {
        if (newPoint.y>highestPoint.y)
        {
            highestPoint = new Vector3(newPoint.x, newPoint.y, newPoint.z);
            Debug.Log($"New Highest point  {highestPoint}");
        }
    }
    
    

    List<int> GetFaces()
    {
        List<int> faces = new List<int>();

        // list of all faces loaded in from custom JSON
        var facesJson = config_script.faces;

        foreach (var face in facesJson)
        {
            faces.Add((int)face[0]-1);
            faces.Add((int)face[2]-1);
            faces.Add((int)face[1]-1);
        }
        return faces;
    }

}
