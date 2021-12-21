using System.Collections.Generic;

using UnityEngine;


public class hrrLoader : MonoBehaviour
{

    private GameObject fireObj;
    public GameObject smv_reader;
    public smvReader smvReaderObj;
    

    private float previousTime = -0.1f;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        fireObj = smvReaderObj.firePrefab;

    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        var time =     smvReaderObj.qFileTimeInUse;
        var hrrCache = smvReaderObj.hrrCache;
        if (hrrCache.ContainsKey(time) && time > previousTime)
        {
            previousTime = time;
            for (int l = 0; l < hrrCache[time].Length; l++)
            {
                var firePoint = hrrCache[time][l];
                int k = firePoint.Z;
                int j = firePoint.Y;
                int i = firePoint.X;
                GameObject s = Instantiate(fireObj,
                    new Vector3(k, i, j), Quaternion.identity);
                s.transform.localScale = new Vector3(2,2,2);
                s.GetComponent<Renderer>().material.SetColor("_TintColor",firePoint.Color);
            }
        }
    }
}