using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class windStreams : MonoBehaviour
{
    // Start is called before the first frame update
    public String json_fileName;

    void Start()
    {
        string[] wordlist =  System.IO.File.ReadAllLines("Data\\fds\\wind_1.json")[0].Split(',');

        List<Vector3> pos = new List<Vector3>();
        Debug.Log(wordlist.Length.ToString());
        for (int i = 1; i < wordlist.Length; i+=4)
        {
            var x = float.Parse(wordlist[i]);
            var z = float.Parse(wordlist[i + 1]);
            var y =float.Parse(wordlist[i + 2]);
            Vector3 temp = new Vector3(x, y, z);
            Debug.Log(temp.ToString());
            pos.Add(temp);
            Debug.Log($"{wordlist[i]}  {wordlist[i+1]}   {wordlist[i+2]}   {pos.Count.ToString()} {i.ToString()}");

        }

        var lengthOfLineRenderer = wordlist.Length / 4;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = lengthOfLineRenderer;
        var points = new Vector3[lengthOfLineRenderer];

        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            var x = float.Parse(wordlist[i*4]);
            var z = float.Parse(wordlist[i*4 + 1]);
            var y =float.Parse(wordlist[i*4 + 2]);
            points[i] = new Vector3(x,y,z);
        }
        lineRenderer.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
