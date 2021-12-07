using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class windStreams : MonoBehaviour
{
    // Start is called before the first frame update
    public String fileDirName;
    private LineRenderer[] windLineRenderers;
 
    private int t = 0;
    
  
    private Dictionary<float, string> fileNameDict;
    private string[] windFiles;
    private GameObject[] allLines;
    private int masterCounter;
    void Start()
    {
        
    
        windFiles = Directory.GetFiles(fileDirName, $"*.bin",
            SearchOption.TopDirectoryOnly);
        Debug.Log(windFiles.Length);
        using (BinaryReader reader = new BinaryReader(File.Open(windFiles[0], FileMode.Open)))
        {

            masterCounter = reader.ReadInt32();

            allLines = new GameObject[masterCounter];
            for (int i = 0; i < masterCounter; i++)
            {
                allLines[i] = new GameObject();
                allLines[i].AddComponent<LineRenderer>();
            }
        }




    }

    void readInData(string fileName)
    {
        var time = Time.timeSinceLevelLoad;

        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {

            var windStreamCount = reader.ReadInt32();
            
            Debug.Log(windStreamCount.ToString());
            int[] windSreamPoints = new int[windStreamCount];
            Debug.Log($"Counter Values");
            for (int k = 0; k < windStreamCount; k++)
            {
                windSreamPoints[k] = reader.ReadInt32();
                Debug.Log(windSreamPoints[k].ToString());

            }
            
            for (int j = 0; j < windStreamCount; j++)
            {
                var points = new Vector3[windSreamPoints[j]];
                Vector3 prevPoint = Vector3.negativeInfinity;
                for (int i = 0; i < windSreamPoints[j]; i++)
                {
                    var t = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    var z = reader.ReadSingle();
                    var y = reader.ReadSingle();
                    Vector3 temp = new Vector3(x, y, z);
                    Debug.Log(temp.ToString());
                    points[i] = temp;
                    

                }
;
                LineRenderer l = allLines[j].GetComponent<LineRenderer>();
                l.startWidth = 1f;
                l.endWidth = 1f;
                l.positionCount = points.Length;
                l.SetPositions(points.ToArray());
                l.useWorldSpace = true;


     //GL.Color(Color.cyan);
      //GL.Vertex3(prevPoint.x, prevPoint.y, prevPoint.z);
      //GL.Vertex3(temp.x, temp.y, temp.z);
      //GL.End();
      //Debug.Log($"{i} Drawn {temp.ToString()}");



            }


            
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
        readInData(windFiles[t]);
        t = (t + 1) % masterCounter;


    }
}


                 
            