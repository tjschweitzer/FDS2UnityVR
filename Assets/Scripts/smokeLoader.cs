using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        smokeObj = smvReaderObj.cubePrefab;
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



                var firePoint = smokeCache[time][l];

                int k =  firePoint.Z;
                int j =  firePoint.Y;
                int i =  firePoint.X;
                float datum = firePoint.Datum;
                //Debug.Log($"{i}  {j}  {k}  {data[i, j, k, 4]}");
                // GameObject s = Instantiate(fireObj, new Vector3((k * meshData["xSize"]) + meshData["xMin"], (i * meshData["zSize"]) + meshData["zMin"], (j * meshData["ySize"]) + meshData["yMin"]), Quaternion.identity);
                GameObject s = Instantiate(smokeObj,
                    new Vector3(k,i,j), Quaternion.identity);
                //s.name = $"{qFileTimeInUse} {i}  {j}  {k}  {datum}";

                float smokeValue = Mathf.InverseLerp(smokeRange[0], smokeRange[1], datum);
                Color smokeColor = smokeGradient.Evaluate(smokeValue);
                var rend = s.GetComponent<Renderer>();
                var _mat = rend.material; 
                _mat.SetColor("_BaseColor", smokeColor);
                s.isStatic = true;
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