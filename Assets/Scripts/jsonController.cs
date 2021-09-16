using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;
using JetBrains.Annotations;

using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

using Newtonsoft.Json;
public class jsonController : MonoBehaviour
{
    // Start is called before the first frame update
    private ConfigData config_script;
    public GameObject configData;
    
    public dynamic faces;
    public dynamic meshData;
    public dynamic treeList;
    public dynamic verts;
    void Awake()
    {
        
            
        config_script= configData.GetComponent<ConfigData>();
        var jsonFileName = config_script.standFireJsonFileName;
        
        string jsonData = "";
        using (StreamReader r = new StreamReader(Path.Combine(jsonFileName)))
        {
            string json = r.ReadToEnd();
            jsonData = json;
        }
        dynamic obj = JsonConvert.DeserializeObject(jsonData);
        verts = obj["verts"];
        meshData = obj["meshData"];
        treeList = obj["treeList"];
        faces = obj["faces"];

    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
