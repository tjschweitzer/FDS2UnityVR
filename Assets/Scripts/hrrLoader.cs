using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hrrLoader : MonoBehaviour
{

    private GameObject fireObj;


    public GameObject smv_reader;
    public smvReader smvReaderObj;
    
    private Dictionary<string,dynamic> meshData = TerrainBuilder.meshData;
    private float[] fireRange;
    private Gradient fireGradient;
    private List<GameObject> previousFrame;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        fireObj = smvReaderObj.cubePrefab;
        fireGradient = smvReaderObj.fireGradient;
        
        fireRange = smvReaderObj.fireRange;
        previousFrame = new List<GameObject>();
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        List<GameObject> currentFrame = new List<GameObject>();
        var time =     smvReaderObj.qFileTimeInUse;
        var hrrCache = smvReaderObj.hrrCache;
        if (hrrCache.ContainsKey(time))
        {
            for (int l = 0; l < hrrCache[time].Length; l++)
            {



                var firePoint = hrrCache[time][l];

                int k =  firePoint.Z;
                int j =  firePoint.Y;
                int i =  firePoint.X;
                float datum = firePoint.Datum;
                //Debug.Log($"{i}  {j}  {k}  {data[i, j, k, 4]}");
                // GameObject s = Instantiate(fireObj, new Vector3((k * meshData["xSize"]) + meshData["xMin"], (i * meshData["zSize"]) + meshData["zMin"], (j * meshData["ySize"]) + meshData["yMin"]), Quaternion.identity);
                GameObject s = Instantiate(fireObj,
                    new Vector3(k,i,j), Quaternion.identity);
                //s.name = $"{qFileTimeInUse} {i}  {j}  {k}  {datum}";

                float fireValue = Mathf.InverseLerp(fireRange[0], fireRange[1], datum);
                Color fireColor = fireGradient.Evaluate(fireValue);
                s.GetComponent<Renderer>().material.SetColor("_Color", fireColor);
                currentFrame.Add(s);

            }

            for (int i = 0; i < previousFrame.Count; i++)
            {
                Destroy(previousFrame[i]);
            }

            previousFrame = new List<GameObject>(currentFrame);
        }
    }
}