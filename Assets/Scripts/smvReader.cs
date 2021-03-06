using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;
using Microsoft.CSharp;

[Serializable]
public class DataPoint
{
    public DataPoint(float X, float Y, float Z, float Datum,Color color)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.Datum = Datum;
        this.Color = color;
    }
    public float X;
    public float Y;
    public float Z;
    public float Datum;
    public Color Color;
}


[Serializable]
public class BaseData
{
    public BaseData(DataPoint[] fire, DataPoint[] smoke)
    {
        this.fire = fire;
        this.smoke = smoke;
    }

    public DataPoint[] fire;
    public DataPoint[] smoke;
}

public class smvReader : MonoBehaviour
{
    // Start is called before the first frame update
    //  public Dictionary<String, NDarray> qDict = new Dictionary<string, NDarray>();

    public LinkedList<string> fileLL;
    public LinkedList<string> jsonFileLL;
    public GameObject firePrefab;
    public GameObject smokePrefab;
    public GameObject cubePrefab;

    public float[,,,] data;
    private bool realFlames;
    public GameObject configData;
    private ConfigData config_script;
    private GameObject usedFirePrefab;
    private String currentFireTag = "Fire1";
    public float[] fireRange = new float[2] {20, 1300};
    public float[] smokeRange = new float[2] {1.2f, 14};

    private Dictionary<string,dynamic> meshData = TerrainBuilder.meshData;
    [FormerlySerializedAs("fire_Gradient")]
    public Gradient fireGradient;

    public Gradient smokeGradient;
    public bool readfdsJson = true;

    public SteamVR_Action_Boolean fireTypeInput;


    public string FireSmokeOption;

    void Start()
    {

        config_script = configData.GetComponent<ConfigData>();

        var targetDirectory = config_script.pl3dDataDir;
        realFlames = config_script.realisticFlames;
        FireSmokeOption = config_script.getFireSmokeOption();
        if (FireSmokeOption == "NoSmokeFire")
        {
            gameObject.SetActive(false);
            return;
        }
        string[] fileEntries =
            Directory.GetFiles(targetDirectory, $"*{TerrainBuilder.CHID}*.bin", SearchOption.TopDirectoryOnly);

        string[] jsonFileEntries = Directory.GetFiles(targetDirectory, $"*{TerrainBuilder.CHID}*.json",
            SearchOption.TopDirectoryOnly);



        
        usedFirePrefab = realFlames ? firePrefab : cubePrefab;
        


        fileEntries = sortedFileArray(fileEntries);
        fileLL = new LinkedList<string>(fileEntries);
        optimizedFDSLoader();
        fileEntries = sortedFileArray(fileEntries);

        fileLL = new LinkedList<string>(fileEntries);
    

    }

    public Dictionary<String, dynamic> getMeshData()
    {
        return meshData;
    }

    string[] sortedFileArray(string[] fileArray)
    {
        List<string> fileList = fileArray.ToList();
        Dictionary<float, string> fileTimeDict = new Dictionary<float, string>();

        for (int i = 0; i < fileList.Count; i++)
        {
            fileTimeDict[getFileTime(fileList[i])] = fileList[i];
        }


        List<float> floatFileName = fileTimeDict.Keys.ToList();
        floatFileName.Sort();

        fileList.Clear();
        for (int i = 0; i < floatFileName.Count; i++)
        {
            fileList.Add(fileTimeDict[floatFileName[i]]);
        }
        
        return fileList.ToArray();
    }

    public float getFileTime(string filename)
    {
        filename = Path.GetFileName(filename);
        var breakDown = filename.Split('.')[0].Split('_');
        int fnLength = breakDown.Length;
        float hundredthsOfSecond = float.Parse(breakDown[fnLength - 1]) / 100.0f;
        float fullTime = float.Parse(breakDown[fnLength - 2]) + hundredthsOfSecond;

        return fullTime;
    }



    // Update is called once per frame
    void Update()
    {
        optimizedFireLoader();
    }

    public string qFilenameInUse;
    public float qFileTimeInUse;

    public Vector3 getQWindVector3(Vector3 particleVelocity, Vector3 particlePosition)
    {
        var xmin = TerrainBuilder.meshData["xmin"];
        var ymin = TerrainBuilder.meshData["ymin"];
        var zmin = TerrainBuilder.meshData["zmin"];
        var xmax = TerrainBuilder.meshData["xmax"];
        var ymax = TerrainBuilder.meshData["ymax"];
        var zmax = TerrainBuilder.meshData["zmax"];
        var xcellSize = (xmax - xmin) / TerrainBuilder.meshData["I"];
        var ycellSize = (ymax - ymin) / TerrainBuilder.meshData["J"];
        var zcellSize = (zmax - zmin) / TerrainBuilder.meshData["K"];



        // Inital Frame mat not have qfile loaded yet.
        if (object.ReferenceEquals(data, null))
        {
            return particleVelocity;
        }



        if (xmin >= particlePosition.x || particlePosition.x >= xmax)
        {
            //Debug.Log($"X out of bounds  {xmin >= particlePosition.x }   { particlePosition.x >= xmax}    {xmin} { particlePosition.x} {xmax}");
            return particleVelocity;
        }

        if (zmin >= particlePosition.z || particlePosition.z >= zmax)
        {

            // Debug.Log($"z out of bounds  {zmin >= particlePosition.z }   { particlePosition.z >= zmax}    {zmin} { particlePosition.z} {zmax}");
            return particleVelocity;
        }

        if (ymin >= particlePosition.y || particlePosition.y >= ymax)
        {
            //Debug.Log($"y out of bounds  {ymin >= particlePosition.y}   { particlePosition.y >= ymax}    {ymin} { particlePosition.y} {ymax}");
            return particleVelocity;
        }

        int x_index = (int) ((particlePosition.x - xmin) / xcellSize);
        int z_index = (int) ((particlePosition.z - zmin) / zcellSize);
        int y_index = (int) ((particlePosition.y - ymin) / ycellSize);

        var qWindVectors = new Vector3(data[z_index, y_index, x_index, 1], data[z_index, y_index, x_index, 3],
            data[z_index, y_index, x_index, 2]);
        //Debug.Log($"Particle Speed x {x_index} y {y_index}, z { z_index}");
        //Debug.Log($"Particle Speed {qWindVectors}");
        if (qWindVectors.x > 0.0f || qWindVectors.z > 0.0f || qWindVectors.y > 0.0f)
        {
            //Debug.Log($" Wind {qWindVectors}");
        }

        var returnValue = qWindVectors;
        return returnValue;
    }

    public Dictionary<float, DataPoint[]> hrrCache;
    public Dictionary<float, DataPoint[]> smokeCache;

    private void optimizedFDSLoader()
    {
        hrrCache = new Dictionary<float, DataPoint[]>();
        smokeCache = new Dictionary<float, DataPoint[]>();

        float[] maxValues = new float[5];

        float[] minValues = new float[5];

        var linkedListCopy = new LinkedList<string>(fileLL);
        while (linkedListCopy.Count > 0)
        {
            qFilenameInUse = linkedListCopy.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            linkedListCopy.RemoveFirst();

            using (BinaryReader reader = new BinaryReader(File.Open(qFilenameInUse, FileMode.Open)))
            {

                var sootCount = reader.ReadInt32();
                var uVelovityCount = reader.ReadInt32();
                var vVelovityCount = reader.ReadInt32();
                var wVelovityCount = reader.ReadInt32();
                var hrrCount = reader.ReadInt32();


                for (int k = 0; k < 5; k++)
                {
                    minValues[k] = reader.ReadSingle();

                }

                for (int k = 0; k < 5; k++)
                {


                    maxValues[k] = reader.ReadSingle();

                }




                for (int k = 0; k < sootCount; k++)
                {
                    var x =  reader.ReadSingle();
                    var y =  reader.ReadSingle();
                    var z =  reader.ReadSingle();
                    var d = reader.ReadSingle();

                    if (!smokeCache.ContainsKey(qFileTimeInUse))
                    {
                        smokeCache[qFileTimeInUse] = new DataPoint[sootCount];
                    }

                    x = (x * meshData["xSize"]) + meshData["xMin"];
                    z = (z * meshData["zSize"]) + meshData["zMin"];
                    y = (y * meshData["ySize"]) + meshData["yMin"];
                    float smokeValue = Mathf.InverseLerp(fireRange[0], fireRange[1], d);
                    Color smokeColor = smokeGradient.Evaluate(smokeValue);
                    
                    smokeCache[qFileTimeInUse][k] = new DataPoint(x, y, z, d,smokeColor);
                }


                for (int k = 0; k < uVelovityCount; k++)
                {
                    var x = (int) reader.ReadSingle();
                    var y = (int) reader.ReadSingle();
                    var z = (int) reader.ReadSingle();
                    var d = (int) reader.ReadSingle();

                    //if ( hrrCache.ContainsKey(qFileTimeInUse))
                    //{
                    //    hrrCache[qFileTimeInUse] = new DataPoint[sootCount];
                    //}

                    //hrrCache[qFileTimeInUse][k]=new DataPoint(x,y,z,d);
                }

                for (int k = 0; k < vVelovityCount; k++)
                {
                    var x = (int) reader.ReadSingle();
                    var y = (int) reader.ReadSingle();
                    var z = (int) reader.ReadSingle();
                    var d = (int) reader.ReadSingle();

                    //if ( hrrCache.ContainsKey(qFileTimeInUse))
                    //{
                    //    hrrCache[qFileTimeInUse] = new DataPoint[sootCount];
                    //}

                    //hrrCache[qFileTimeInUse][k]=new DataPoint(x,y,z,d);
                }

                for (int k = 0; k < wVelovityCount; k++)
                {
                    var x = (int) reader.ReadSingle();
                    var y = (int) reader.ReadSingle();
                    var z = (int) reader.ReadSingle();
                    var d = reader.ReadSingle();

                    //if ( hrrCache.ContainsKey(qFileTimeInUse))
                    //{
                    //    hrrCache[qFileTimeInUse] = new DataPoint[sootCount];
                    //}

                    //hrrCache[qFileTimeInUse][k]=new DataPoint(x,y,z,d)
                }

                for (int k = 0; k < hrrCount; k++)
                {
                    var x =  reader.ReadSingle();
                    var y =  reader.ReadSingle();
                    var z =  reader.ReadSingle();
                    var d = reader.ReadSingle();
                    if (!hrrCache.ContainsKey(qFileTimeInUse))
                    {
                        hrrCache[qFileTimeInUse] = new DataPoint[hrrCount];
                    }


                    x = (x * meshData["xSize"]) + meshData["xMin"];
                    z = (z * meshData["zSize"]) + meshData["zMin"];
                    y = (y * meshData["ySize"]) + meshData["yMin"];
                    float fireValue = Mathf.InverseLerp(fireRange[0], fireRange[1], d);
                    Color fireColor = fireGradient.Evaluate(fireValue);

                    hrrCache[qFileTimeInUse][k] = new DataPoint(x, y, z, d,fireColor);

                }

            }


            fireRange[1] = maxValues[4];
            fireRange[0] = minValues[4];
            smokeRange[1] = maxValues[0];
            smokeRange[0] = minValues[0];
        }
    }


    void optimizedFireLoader()
    {

        //Debug.Log($"Json Length {jsonFileLL.Count}");

        List<GameObject> currentFrameObjects = new List<GameObject>();
        if (fileLL.Count >= 1)
        {

            var worldTime = Time.timeSinceLevelLoad;


            qFilenameInUse = fileLL.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            // Debug.Log($"Lading Voxals {worldTime} QTime {qFileTimeInUse} LL SIZE {fileLL.Count}");
            if (qFileTimeInUse < worldTime)
            {


                fileLL.RemoveFirst();
                while (qFileTimeInUse < worldTime && fileLL.Count > 1)
                {
                    qFilenameInUse = fileLL.First.Value;
                    qFileTimeInUse = getFileTime(qFilenameInUse);
                    fileLL.RemoveFirst();
                }
         
            }
        }
    }

}