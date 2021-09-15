using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Valve.Newtonsoft.Json.Linq;


public class TreeLoader : MonoBehaviour
{

    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.

    //  treePrefab Top of tree
    public GameObject treePrefab;
    //  treeBase Trunk of tree
    public GameObject treeBase;
  
    
    public GameObject configData;
    private ConfigData config_script;
    private string fileName;
    void Start()
    {
        config_script= configData.GetComponent<ConfigData>();
        Debug.Log("FDS Tree Loader Started ");
        
        // filename No longer Used
        fileName = config_script.fileName;

        fdsReader();

        Debug.Log("FDS Tree Loader Ended");


    }

    
    void fdsReader()
    {
        // Grabs the inital size of the tree crown 
        float treePrefabx =  treePrefab.GetComponent<Renderer>().bounds.size.x;
        float treePrefaby =  treePrefab.GetComponent<Renderer>().bounds.size.y;
        float treePrefabz = treePrefab.GetComponent<Renderer>().bounds.size.z;
       
        
        // list of all trees loaded in from custom JSON
        var treeList = config_script.treeList;
        //Parent game object that all trees will be the children of
        GameObject allTrees = new GameObject("allTrees");
        foreach (var tree in treeList)
        {
            Debug.Log(tree.ToString());
            float x =(float) tree["x"];
            float y =(float) tree["y"];
            float z = (float)tree["crownBaseHeight"];
            float treeDiameter = (float)tree["crownRadius"]*2;
            float treeHeight = (float)tree["crownHeight"];  // Height of the tree from base of the crown to top
           // float groundHeight = (float) tree["groundHeight"]; // Elevation of topography 
            float crownBaseHeight = (float) tree["crownBaseHeight"]; // Elevation + Height to base of tree crown
            var treeBaseMidpoint =crownBaseHeight-(treeHeight/2.0f);  // half way up the tree trunk for placement
            // Skips any tree with a diameter less then 1

            
            // Creates the tree crown
            GameObject treeObj = Instantiate(treePrefab, new Vector3(x, z, y), Quaternion.identity);
            // Resizes the tree crown
            treeObj.transform.localScale = new Vector3( treeDiameter / treePrefabx, treeHeight / treePrefaby, treeDiameter / treePrefabz);
            // Sets Tree crown as the child of "allTrees" game object
            treeObj.transform.SetParent(allTrees.transform);
            
            Debug.Log($"Tree Height {treeHeight}  ");
            
            
            // Creates the tree trunk
            GameObject baseTree =
                Instantiate(treeBase, new Vector3(x, treeBaseMidpoint, y), Quaternion.identity);
            // Resizes the tree trunk
            baseTree.transform.localScale = new Vector3(treeDiameter / (4*treePrefabx) , treeHeight/1.8f ,
                treeDiameter / (4*treePrefabz) );
            // Sets Tree trunk as the child of "allTrees" game object
            baseTree.transform.SetParent(allTrees.transform);
            
        }
    }

    void JsonReader(){
    // Instantiate at position (0, 0, 0) and zero rotation.

        float treePrefabx =  treePrefab.GetComponent<Renderer>().bounds.size.x;
        float treePrefaby =  treePrefab.GetComponent<Renderer>().bounds.size.y;
        float treePrefabz = treePrefab.GetComponent<Renderer>().bounds.size.z;
          
        // x,y,height,crownHeight,crownBaseHeight,crownRadius
    
        JObject treeJson = JObject.Parse(File.ReadAllText(fileName));
        GameObject allPlots = new GameObject("allPlots");
        foreach (var treeJsonPlots in treeJson["plots"])
        {
            float plotx = float.Parse(treeJsonPlots["x"].ToString());

            float ploty = float.Parse(treeJsonPlots["y"].ToString());
            GameObject plot = new GameObject(treeJsonPlots["pltId"].ToString());

            plot.transform.SetParent(allPlots.transform);
            foreach (var treeJsonTree in treeJsonPlots["trees"])
            {

                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (var kv in treeJsonTree.Children())
                {
                    string key = kv.ToString().Split(':')[0].Replace("\"", "");
                    string value = kv.ToString().Split(':')[1].Replace("\"", "");
                    values[key] = value;
                }

                float x = float.Parse(values["x"]) + plotx;
                float y = float.Parse(values["y"]) + ploty;
                float height = float.Parse(values["height"]);
                float crownHeight = float.Parse(values["crownHeight"]);
                float crownBaseHeight = float.Parse(values["crownBaseHeight"]);
                float crownRadius = float.Parse(values["crownRadius"]);


                GameObject tree = Instantiate(treePrefab, new Vector3(x, crownBaseHeight, y), Quaternion.identity);
                tree.transform.localScale = new Vector3(2 * crownRadius / treePrefabx, crownHeight / treePrefaby,
                    2 * crownRadius / treePrefabz);

                tree.transform.SetParent(plot.transform);
                GameObject baseTree =
                    Instantiate(treeBase, new Vector3(x, crownBaseHeight / 2, y), Quaternion.identity);
                baseTree.transform.localScale = new Vector3(crownRadius / treePrefabx / 2, crownBaseHeight / 2,
                    crownRadius / treePrefabz / 2);

                baseTree.transform.SetParent(plot.transform);
            }
        }

            
    
    }

}