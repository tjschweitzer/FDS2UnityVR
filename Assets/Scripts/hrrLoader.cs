using System;
using System.Collections.Generic;

using UnityEngine;


public class hrrLoader : MonoBehaviour
{

    private GameObject fireObj;
    public GameObject smv_reader;
    public smvReader smvReaderObj;

    private Dictionary<String,dynamic> meshData;

    private float previousTime = -0.1f;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        fireObj = smvReaderObj.firePrefab;
        meshData = smvReaderObj.getMeshData();

    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        var time =     smvReaderObj.qFileTimeInUse;
        var hrrCache = smvReaderObj.hrrCache;
        if (hrrCache.ContainsKey(time) && time > previousTime && smvReaderObj.FireSmokeOption=="Fire")
        {
            previousTime = time;
            for (int l = 0; l < hrrCache[time].Length; l++)
            {
                var firePoint = hrrCache[time][l];
                float k = firePoint.Z;
                float j = firePoint.Y;
                float i = firePoint.X;
                GameObject s = Instantiate(fireObj,
                    new Vector3(k, i, j), Quaternion.identity);
                s.transform.localScale = new Vector3(meshData["xSize"],meshData["zSize"],meshData["ySize"]);
                s.GetComponent<Renderer>().material.SetColor("_TintColor",firePoint.Color);
            }
        }
    }
}