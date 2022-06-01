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
    private int counter = 0;
    
    public ParticleSystem particleSystem;
    private Dictionary<string,float> fileNameDict;
    private string[] windFiles;
    private GameObject[] allLines;
    private int masterCounter;
    public GameObject defaultWindObject;
    public Gradient windValueGradient;

    private Vector4[] current_wind_particles;


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

    void moveParticle()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount+1];
        int l = particleSystem.GetParticles(p);
        for (int i = 0; i < p.Length; i++)
        {
            var currentPosition = p[i].position;
            //p[i].velocity = 
            
             p[i].velocity = new Vector3(0, p[i].remainingLifetime / p[i].startLifetime * 10F, 0);

        }
  
        particleSystem.SetParticles(p, l);    
    }


   
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
