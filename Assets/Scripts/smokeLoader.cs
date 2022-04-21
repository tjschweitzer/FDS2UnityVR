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
    private Dictionary<String, dynamic> meshData;

    private float previousTime= -0.1f;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        smokeObj = smvReaderObj.smokePrefab;
        
        meshData = smvReaderObj.getMeshData();
    }

    private void Update()
    {
    }
    
    private void LateUpdate()
    {
        var time =     smvReaderObj.qFileTimeInUse;
        var smokeCache = smvReaderObj.smokeCache;
        if (smokeCache.ContainsKey(time) && time > previousTime && smvReaderObj.FireSmokeOption=="Smoke" )
        {
            previousTime = time;
            for (int l = 0; l < smokeCache[time].Length; l++)
            {
                var smokePoint = smokeCache[time][l];
                float k =  smokePoint.Z;
                float j =  smokePoint.Y;
                float i =  smokePoint.X;
               GameObject s = Instantiate(smokeObj,
                    new Vector3(k,i,j), Quaternion.identity);
               s.transform.localScale = new Vector3(meshData["xSize"],meshData["zSize"],meshData["ySize"]);
               var t = s.GetComponent<ParticleSystem>().main;
               t.startColor = new ParticleSystem.MinMaxGradient(smokePoint.Color,smokePoint.Color);
            }
        }
    }
}