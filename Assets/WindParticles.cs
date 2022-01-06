using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WindParticles : MonoBehaviour
{
    public GameObject configData;
    private ConfigData config_script;
    public String fileDirName;
    private LineRenderer[] windLineRenderers;
 
    private int t = 0;
    
  
    private Dictionary<string,float> fileNameDict;
    private string[] windFiles;
    private GameObject[] allLines;
    private int masterCounter;
    public GameObject defaultWindObject;
    public Gradient windValueGradient;

    void Start()
    {
        config_script = configData.GetComponent<ConfigData>();
        if (config_script.pl3dDataDir is null)
        {
            Debug.Log("Debug Path");
        }
        else
        {



            fileDirName = @$"{config_script.pl3dDataDir}\wind";
        }

        windFiles = Directory.GetFiles(fileDirName, $"*.bin",
            SearchOption.TopDirectoryOnly);

        using (BinaryReader reader = new BinaryReader(File.Open(windFiles[0], FileMode.Open)))
        {
            var blank = reader.ReadSingle();
            masterCounter = reader.ReadInt32();

            allLines = new GameObject[masterCounter];
            for (int i = 0; i < masterCounter; i++)
            {
                allLines[i] = new GameObject();

                GameObject s = Instantiate(defaultWindObject,
                    new Vector3(0, 0, 0), Quaternion.identity);
                allLines[i] = s;
            }
        }
        windFiles = sortedFileArray(windFiles);

        fileNameDict = new Dictionary<string, float>();
        for (int i = 0; i < windFiles.Length; i++)
        {
            fileNameDict[windFiles[i]] = getFileTime(windFiles[i]);
        }


    }
    
    
    float getFileTime(string filename)
    {
        var breakDown = filename.Split('.')[0].Split('_');
        int fnLength = breakDown.Length;
        float hundredthsOfSecond = float.Parse(breakDown[fnLength - 1]) / 100.0f;
        float fullTime = float.Parse(breakDown[fnLength - 2]) + hundredthsOfSecond;
        return fullTime;
    }

    string[] sortedFileArray(string[] fileArray)
    {
        List<string> fileList = fileArray.ToList();
        Dictionary<float, string> fileTimeDict = new Dictionary<float, string>();

        for (int i = 0; i < fileList.Count; i++)
        {
            fileTimeDict[getFileTime(fileList[i])] = fileList[i];
        }
        List<float> floatFileName = fileTimeDict.Keys.ToList();
        floatFileName.Sort();
        fileList.Clear();
        for (int i = 0; i < floatFileName.Count; i++)
        {
            fileList.Add(fileTimeDict[floatFileName[i]]);
        }
        return fileList.ToArray();
    }

    void readInData(string fileName)
    {
        var time = Time.timeSinceLevelLoad;

        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {
            var maxVel = reader.ReadSingle();
            var windStreamCount = reader.ReadInt32();
            
            int[] windSreamPoints = new int[windStreamCount];

            for (int k = 0; k < windStreamCount; k++)
            {
                windSreamPoints[k] = reader.ReadInt32();

            }
            
            for (int j = 0; j < windStreamCount; j++)
            {
                var points = new Vector3[windSreamPoints[j]];
                
                var texture = new Texture2D(windSreamPoints[j], 1, TextureFormat.ARGB32, false);
                
                for (int i = 0; i < windSreamPoints[j]; i++)
                {
                    var d = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    var z = reader.ReadSingle();
                    var y = reader.ReadSingle();

                    
                    float windValue = Mathf.InverseLerp(0.0f, maxVel, d);
                    Color windColor = windValueGradient.Evaluate(windValue);
                    
                    texture.SetPixel( i,0, windColor);
                    Vector3 temp = new Vector3(x, y, z);
                    
                    points[i] = temp;
                }
                texture.Apply();
                LineRenderer l = allLines[j].GetComponent<LineRenderer>();
                l.startWidth = .5f;
                l.endWidth = .5f;
                l.positionCount = points.Length;
                l.SetPositions(points.ToArray());
                l.useWorldSpace = true;

     
                l.material = new Material(Shader.Find("Sprites/Default"));
                l.GetComponent<Renderer>().material.mainTexture = texture;
            }
        }
    }
    // Update is called once per frame
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
