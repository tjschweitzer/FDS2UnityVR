using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
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

        // Fix scaling errors
        if (treePrefabx == 0.0f) { treePrefabx = 1.0f; }
        if (treePrefaby == 0.0f) { treePrefaby = 1.0f; }
        if (treePrefabz == 0.0f) { treePrefabz = 1.0f; }
        // list of all trees loaded in from custom JSON
        var treeList = config_script.treeList;
        //Parent game object that all trees will be the children of
        GameObject allTrees = new GameObject("allTrees");
        for (int i = 0; i < treeList.Count; i++) {
            var tree = treeList[i];
            // Debug.Log(tree.ToString());
            float x =(float) tree["x"];
            float y =(float) tree["y"];
            float z = (float)tree["crownBaseHeight"];
            float treeDiameter = (float)tree["crownRadius"]*2;
            float treeHeight = (float)tree["crownHeight"];  // Height of the tree from base of the crown to top
            float groundHeight = (float) tree["height"]; // Elevation of topography 
            float crownBaseHeight = (float) tree["crownBaseHeight"]; // Elevation + Height to base of tree crown
            var treeBaseMidpoint =crownBaseHeight-(treeHeight/2.0f);  // half way up the tree trunk for placement
            // Skips any tree with a diameter less then 1

            
            // Creates the tree crown
            GameObject treeObj = Instantiate(treePrefab, new Vector3(x, z, y), Quaternion.identity);
            // Resizes the tree crown
            treeObj.transform.localScale = new Vector3( treeDiameter / treePrefabx, treeHeight / treePrefaby, treeDiameter / treePrefabz);
            // Sets Tree crown as the child of "allTrees" game object
            treeObj.transform.SetParent(allTrees.transform);
            
            // Debug.Log($"Tree Height {treeHeight}  ");
            
            
            // Creates the tree trunk
            GameObject baseTree =
                Instantiate(treeBase, new Vector3(x, z/2.0f, y), Quaternion.identity);
            // Resizes the tree trunk
            baseTree.transform.localScale = new Vector3(treeDiameter / (4*treePrefabx) , z/2.0f ,
                treeDiameter / (4*treePrefabz) );
            // Sets Tree trunk as the child of "allTrees" game object
            baseTree.transform.SetParent(allTrees.transform);
            
        }
    }


}