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
    private float previousTime = -0.1f;
    private void Start()
    {
        smvReaderObj = smv_reader.GetComponent<smvReader>();
        fireObj = smvReaderObj.firePrefab;
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

                var t = s.GetComponent<ParticleSystem>().main;
                t.startColor = new ParticleSystem.MinMaxGradient(firePoint.Color,firePoint.Color);
                currentFrame.Add(s);

            }
        

        }
    }
}