using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireDict : MonoBehaviour
{
    
    public GameObject firePrefab;
    // Start is called before the first frame update
    private Dictionary<Vector3, GameObject> fireDict;
    private Dictionary<String, Dictionary<Vector3, float>> allFireData;
    private List<String> listOfKeys;
    private int counter = 0;
    void Start()
    {

        fireDict = new Dictionary<Vector3, GameObject>();
        for (int i = 0; i < 10; i++)
        {

            Vector3 position = new Vector3(i, 2, 0);
            GameObject s = Instantiate(firePrefab,
                position, Quaternion.identity);


            Color fireColor = GETColorValue(2);
            s.GetComponent<Renderer>().material.SetColor("_Color", fireColor);
            fireDict[position] = s;
        }

        listOfKeys = new List<string>();
        allFireData = new Dictionary<string, Dictionary<Vector3, float>>();
        for (int i = 3; i < 20; i++)
        {
            allFireData[i.ToString()] = new Dictionary<Vector3, float>();
            listOfKeys.Add(i.ToString());
            for (int j = 0; j < 10; j++)
            {
                Vector3 position = new Vector3(j, 2, 0);
                allFireData[i.ToString()][position] = i;
            }
        }

        

    }
    
    Color GETColorValue(float dataValue)
    {
        //Debug.Log("Value " +dataValue+"% Value "+minGreenValue)
        var minGreenValue = dataValue / 20;
        var alphaValue = minGreenValue;
        return new Color(1.0f,minGreenValue, 0.0f,  alphaValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (listOfKeys.Count< counter+1)
        {
            return;
        }
        var currentKey = listOfKeys[counter];
        counter++;
        
        var currentFires = allFireData[currentKey];
        foreach (var keyValue in currentFires)
        {
            
            Color fireColor = GETColorValue(keyValue.Value);
            fireDict[ keyValue.Key].GetComponent<Renderer>().material.SetColor("_Color", fireColor);
            fireDict[keyValue.Key].transform.position =
                new Vector3(keyValue.Key.x, keyValue.Key.y +counter, keyValue.Key.z);
        }

    }
}
