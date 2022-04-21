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

    public string fileName;
    public string pl3dDataDir;
    public string windDataDir;
    public bool fastFuels = false;
    public bool pauseGame = false;



    public string standFireJsonFileName = @"E:\TestData\sample.json";


    public bool smoothTerrain = false;
    public bool realisticFlames = false;

    public string[] terrainLabels = new[] { "wet vegetation" };
    public string[] obstacleLabels = new[] { "Urban", "Snow-Ice", "Agriculture", "Water", "Barren", "NA" };


    public dynamic faces;
    public dynamic meshData;
    public dynamic treeList;
    public dynamic verts;

    private bool _loadTrees;
    private string _loadFireSmokeOption;
    private string _loadWindOption;

    void Awake()
    {
        if (MainMenu.FdsPath != null)
        {
            fileName = MainMenu.FdsPath;
        }

        if (MainMenu.BinPath != null)
        {
            pl3dDataDir = MainMenu.BinPath;
        }

        if (MainMenu.WindPath != null)
        {
            windDataDir = MainMenu.WindPath;
        }
        
        _loadTrees = true;
        if (MainMenu.TreesActive != null)
        {
           setLoadTrees( MainMenu.TreesActive);
        }

        _loadFireSmokeOption = "Fire";
        if (MainMenu.FireSmokeOption != null)
        {
            setFireSmokeOption( MainMenu.FireSmokeOption);
        }
        _loadWindOption = "";
        if (MainMenu.WindOption != null)
        {
            setWindOption(MainMenu.WindOption);
;
        }
        
        pauseGame = false;
        var jsonFileName = standFireJsonFileName;
        if (MainMenu.JsonPath != null)
        {
            jsonFileName = MainMenu.JsonPath;
        }

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

    
    
    public string getFireSmokeOption()
    {
        return _loadFireSmokeOption;
    }

    private void setFireSmokeOption(string value)
    {
        _loadFireSmokeOption = value;
    }
    public bool getLoadTrees()
    {
        return _loadTrees;
    }

    private void setLoadTrees(bool value)
    {
        _loadTrees = value;
    }
    public string getWindOption()
    {
        return _loadWindOption;
    }

    private void setWindOption(string value)
    {
        _loadWindOption = value;
    }

    private void Update()
    {
    }
}