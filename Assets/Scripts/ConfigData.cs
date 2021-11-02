using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.Newtonsoft.Json;

public class ConfigData : MonoBehaviour
{
    // Start is called before the first frame update
    
    public string fileName =  @"E:\TestData\treelist.fds";
    public string pl3dDataDir = @"E:\TestData\test4";

    public bool fastFuels = false;

    public bool pauseGame = false;
    public bool activeSmokeStatus = false;
    
    public string standFireJsonFileName = @"E:\TestData\sample.json";
    
    //public string fileName =  @"/home/kl3pt0/FastFuels_Steam_VR/TestData/fds/testfdsrun.fds";
    //public string pl3dDataDir = @"/home/kl3pt0/FastFuels_Steam_VR/TestData";
   
    public bool smoothTerrain=false;
    public bool realisticFlames = false;

    public string[] terrainLabels = new[] {"wet vegetation"};
    public string[]obstacleLabels = new[] {"Urban","Snow-Ice","Agriculture","Water","Barren","NA"};


    public dynamic faces;
    public dynamic meshData;
    public dynamic treeList;
    public dynamic verts;
    void Awake()
    {
        var jsonFileName =standFireJsonFileName;
        
        string jsonData = "";
        using (StreamReader r = new StreamReader(Path.Combine(jsonFileName)))
        {
            string json = r.ReadToEnd();
            jsonData = json;
        }
        dynamic obj = JsonConvert.DeserializeObject(jsonData);
        verts = obj["verts"];
        
        treeList = obj["treeList"];
        faces = obj["faces"];
        meshData = obj["meshData"];

    }
}
