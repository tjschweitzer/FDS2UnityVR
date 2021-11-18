using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class smokeLoader : MonoBehaviour
{

    private GameObject smokeObj;


    public GameObject smv_reader;
    public smvReader smvReaderObj;
    
    private Dictionary<string,dynamic> meshData = TerrainBuilder.meshData;
    private float[] smokeRange;
    private Gradient smokeGradient;
    private float previousTime= -0.1f;
    private List<GameObject> previousFrame;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        smokeObj = smvReaderObj.smokePrefab;
        smokeGradient = smvReaderObj.smokeGradient;
        
        smokeRange = smvReaderObj.smokeRange;
        previousFrame = new List<GameObject>();

    }

    private void Update()
    {
    }
    
    private void LateUpdate()
    {
        List<GameObject> currentFrame = new List<GameObject>();
        var time =     smvReaderObj.qFileTimeInUse;
        var smokeCache = smvReaderObj.smokeCache;
        if (smokeCache.ContainsKey(time) && time > previousTime )
        {
            previousTime = time;
            for (int l = 0; l < smokeCache[time].Length; l++)
            {



                var smokePoint = smokeCache[time][l];

                int k =  smokePoint.Z;
                int j =  smokePoint.Y;
                int i =  smokePoint.X;
                float datum = smokePoint.Datum;
               GameObject s = Instantiate(smokeObj,
                    new Vector3(k,i,j), Quaternion.identity);
               Renderer rend = s.GetComponent<Renderer>();
               //var t = s.GetComponent<ParticleSystem>().main;
               //t.startColor = new ParticleSystem.MinMaxGradient(smokePoint.Color,smokePoint.Color);
                currentFrame.Add(s);

            }

            for (int i = 0; i < previousFrame.Count; i++)
            {
                //Destroy(previousFrame[i]);
            }

           // previousFrame = new List<GameObject>(currentFrame);
        }
    }
}