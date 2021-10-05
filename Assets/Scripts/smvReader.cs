using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronPython.Modules;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;

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
    public float[] fireRange = new float[2] {20,1300} ;
    public float[] smokeRange = new float[2] {1.2f,14} ;
    
    [FormerlySerializedAs("fire_Gradient")] public Gradient fireGradient;
    public Gradient smokeGradient;
    private float firePrefabx;
    private float firePrefaby;
    private float firePrefabz;
    public GameObject pauseMenu;
    private PauseMenu pauseScript;
    public bool readfdsJson = true;
    
    
    public SteamVR_Action_Boolean fireTypeInput;
    
    
    void Start()
    {
        
        config_script= configData.GetComponent<ConfigData>();
        
        var targetDirectory = config_script.pl3dDataDir;
        realFlames = config_script.realisticFlames;
        
        pauseScript= pauseMenu.GetComponent<PauseMenu>();
       
        string[] fileEntries = Directory.GetFiles(targetDirectory, $"*{TerrainBuilder.CHID}*.q", SearchOption.TopDirectoryOnly);

        string[] jsonFileEntries = Directory.GetFiles(targetDirectory, $"*{TerrainBuilder.CHID}*.json", SearchOption.TopDirectoryOnly);



        usedFirePrefab = realFlames ? firePrefab : cubePrefab;


        firePrefabx = usedFirePrefab.GetComponent<Renderer>().bounds.size.x;
        firePrefaby = usedFirePrefab.GetComponent<Renderer>().bounds.size.y;
        firePrefabz = usedFirePrefab.GetComponent<Renderer>().bounds.size.z;

        if (readfdsJson)
        {
                    
            Debug.Log("Start File Loading");
            //jsonFileEntries = sortedFileArray(jsonFileEntries);
            jsonFileLL = new LinkedList<string>(jsonFileEntries);
            
            readInJson();
            jsonFileEntries = sortedFileArray(jsonFileEntries);
            jsonFileLL = new LinkedList<string>(jsonFileEntries);

            Debug.Log("End File Loading");
        }
        else
        {
            
            fileEntries = sortedFileArray(fileEntries);
            fileLL = new LinkedList<string>(fileEntries);
            optimizedFDSLoader();
        }
    }
    
    string[] sortedFileArray(string[] fileArray)
    {
        List<string> fileList = fileArray.ToList();
        Dictionary<float, string> fileTimeDict = new Dictionary<float, string>();
        
        foreach (var fileName in fileList)
        {
            fileTimeDict[getFileTime(fileName)]=fileName;
        }

        List<float> floatFileName =  fileTimeDict.Keys.ToList();
        floatFileName.Sort();
        
        fileList.Clear();
        foreach (var floatTime in floatFileName)
        {
            fileList.Add(fileTimeDict[floatTime]);
        }



        return fileList.ToArray();
    }
    
    float getFileTime(string filename)
    {
        filename = Path.GetFileName(filename);
        var breakDown = filename.Split('.')[0].Split('_');
        int fnLength = breakDown.Length;
        float hundredthsOfSecond = float.Parse(breakDown[fnLength - 1])/100.0f; 
        float fullTime = float.Parse(breakDown[fnLength - 2])+hundredthsOfSecond; 

        return fullTime;
    }
    
    Color GETColorValue(float dataValue)
    {
        float[] greenFireColorRange = new float[2] {((dataValue - fireRange[0]) / (fireRange[1]-fireRange[0])),1.0f};
        float minGreenValue = greenFireColorRange.Min();
        //Debug.Log("Value " +dataValue+"% Value "+minGreenValue)
        var alphaValue = minGreenValue;
        if (realFlames)
        {
            alphaValue = minGreenValue;
        } 
        return new Color(1.0f,minGreenValue, 0.0f,  alphaValue);
    }
    // Update is called once per frame
    void Update()
    {

            if (realFlames != fireTypeInput.state)
            {
                
                realFlames = !realFlames;
                if (realFlames)
                {
                    usedFirePrefab = firePrefab;
                }
                else
                {
                    usedFirePrefab = cubePrefab;
                }
                firePrefabx = usedFirePrefab.GetComponent<Renderer>().bounds.size.x;
                firePrefaby = usedFirePrefab.GetComponent<Renderer>().bounds.size.y;
                firePrefabz = usedFirePrefab.GetComponent<Renderer>().bounds.size.z;
            }
            StartCoroutine(optimizedFireLoader());
        
    }

    public string qFilenameInUse;
    public float qFileTimeInUse;

    public Vector3 getQWindVector3(Vector3 particleVelocity, Vector3 particlePosition)
    {
        var xmin = TerrainBuilder.meshData["xmin"];
        var  ymin= TerrainBuilder.meshData["ymin"]; 
        var zmin= TerrainBuilder.meshData["zmin"];
        var xmax = TerrainBuilder.meshData["xmax"];
        var  ymax= TerrainBuilder.meshData["ymax"]; 
        var zmax= TerrainBuilder.meshData["zmax"];
        var xcellSize = (xmax - xmin) / TerrainBuilder.meshData["I"];
        var ycellSize = (ymax - ymin) / TerrainBuilder.meshData["J"];
        var zcellSize = (zmax - zmin) / TerrainBuilder.meshData["K"];

        
        
        // Inital Frame mat not have qfile loaded yet.
        if (object.ReferenceEquals( data, null))
        {
            return particleVelocity ;
        }



        if (xmin >= particlePosition.x || particlePosition.x >= xmax)
        {
            //Debug.Log($"X out of bounds  {xmin >= particlePosition.x }   { particlePosition.x >= xmax}    {xmin} { particlePosition.x} {xmax}");
            return particleVelocity ;
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

        var qWindVectors = new Vector3(data[z_index, y_index, x_index,1],data[z_index, y_index, x_index,3],data[z_index, y_index, x_index,2]);
        //Debug.Log($"Particle Speed x {x_index} y {y_index}, z { z_index}");
        //Debug.Log($"Particle Speed {qWindVectors}");
        if (qWindVectors.x> 0.0f|| qWindVectors.z>0.0f || qWindVectors.y>0.0f)
        {
            //Debug.Log($" Wind {qWindVectors}");
        }
        var returnValue = qWindVectors;
        return returnValue;
    }

    private Dictionary<float,List<List<float>>> hrrCache;
    private Dictionary<float,List<List<float>>> smokeCache;
    private void optimizedFDSLoader()
    {
        float maxHRR = 0;
        hrrCache = new Dictionary<float, List<List<float>>>();
        int nx;
        int ny;
        int nz;
        var linkedListCopy = new LinkedList<string>(fileLL);
        while (linkedListCopy.Count>0)
        {
            qFilenameInUse = linkedListCopy.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            linkedListCopy.RemoveFirst();
            Debug.Log(qFilenameInUse);
            using (BinaryReader reader = new BinaryReader(File.Open(qFilenameInUse, FileMode.Open)))
            {

                int _ = reader.ReadInt32();
                nx = reader.ReadInt32();
                ny = reader.ReadInt32();
                nz = reader.ReadInt32();
                //Debug.Log($"{qFilenameInUse} nx ny nz {nx}  {ny}  {nz}");
                _ = reader.ReadInt32();
                //reads in header
                for (int k = 0; k < 7; k++)
                {
                    float _f = reader.ReadSingle();
                }

                bool firstEntry = true;
                int counter = 0;
                data = new float[nz, ny, nx, 5];
                float[,,,] fullFile = new float[nz, ny, nx, 5];

                for (int l = 0; l < 5; l++)
                {
                    for (int i = 0; i < nz; i++)
                    {
                        for (int j = 0; j < ny; j++)
                        {
                            for (int k = 0; k < nx; k++)
                            {
                                var datum = reader.ReadSingle();

                                //fullFile[i, j, k, l] = datum;


                                if (l == 4 && datum > fireRange[0])
                                {
                                    counter++;
                                    if (firstEntry)
                                    {
                                        hrrCache[qFileTimeInUse] = new List<List<float>>();
                                        firstEntry = false;
                                    }

                                    List<float> firePostionXYZData = new List<float>();
                                    firePostionXYZData.Add(i);
                                    firePostionXYZData.Add(j);
                                    firePostionXYZData.Add(k);
                                    firePostionXYZData.Add(datum);
                                    hrrCache[qFileTimeInUse].Add(firePostionXYZData);
                                    if (datum > maxHRR)
                                    {
                                        maxHRR = datum;
                                    }
                                }


                            }
                        }
                    }
                }

                Debug.Log($" {qFilenameInUse} has {counter} voxals");

            }


            fireRange[1] = (int) maxHRR;
        }
    }
    private void readInJson()
    {
        
        
       
        var meshData = TerrainBuilder.meshData;
        float maxHRR = 0;
        float maxSmokeDen = 0;
        hrrCache = new Dictionary<float, List<List<float>>>();
        smokeCache = new Dictionary<float, List<List<float>>>();
        var linkedListCopy = new LinkedList<string>(jsonFileLL);
        
        Debug.Log($"New Time Created  {linkedListCopy.Count}");
        while (linkedListCopy.Count > 0)
        {
            qFilenameInUse = linkedListCopy.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            linkedListCopy.RemoveFirst();
            
            Debug.Log($"New Time Created  {qFilenameInUse} {qFileTimeInUse}");
            if (!hrrCache.ContainsKey(qFileTimeInUse))
            {
                hrrCache[qFileTimeInUse] = new List<List<float>>();
            }
            if (!smokeCache.ContainsKey(qFileTimeInUse))
            {
                smokeCache[qFileTimeInUse] = new List<List<float>>();
            }
            

            string jsonData;
            using (StreamReader r = new StreamReader(Path.Combine(qFilenameInUse)))
            {
                string json = r.ReadToEnd();
                jsonData = json;
            }
            Debug.Log($"{qFilenameInUse}  Loaded");
            dynamic obj = JsonConvert.DeserializeObject(jsonData);


            

            var fireObj = obj["fire"];
            int counter = 0;
            foreach (var point in fireObj)
            {

                string position = point.ToString().Split(':')[0];
                float datum = float.Parse(point.ToString().Split(':')[1].Replace('"', ' '));

                string[] positions = position.Replace('"', ' ').Split('-');
                List<float> firePostionXYZData = new List<float>();

                // Indexed position of voxel in current mesh
                float i = float.Parse(positions[0]);
                float j = float.Parse(positions[1]);
                float k = float.Parse(positions[2]);

                // converts relative index to global indexes
                var temp = qFilenameInUse.Split('_');
                var meshNumber = int.Parse(temp[temp.Length - 3]) - 1;

                Debug.Log($"New Time Created  {qFilenameInUse} {qFileTimeInUse}  {meshNumber}");
                if (meshData["multID"] != String.Empty)
                {


                    var multMeshData = TerrainBuilder.multiData[meshData["multID"]];
                    var meshRow = meshNumber % (multMeshData["I_UPPER"] + 1);
                    var meshCol = Math.Floor(meshNumber / (multMeshData["I_UPPER"] + 1));
                    var meshHeight =
                        Math.Floor(meshNumber / ((multMeshData["I_UPPER"] + 1) * (multMeshData["K_UPPER"] + 1)));

                    i += meshCol * meshData["K"];
                    j += meshHeight * meshData["J"];
                    k += meshRow * meshData["I"];
                    Debug.Log(
                        $"Row {meshRow} of {(multMeshData["I_UPPER"] + 1)} Col {meshCol} of {multMeshData["K_UPPER"] + 1} Height {meshHeight} of {multMeshData["J_UPPER"] + 1}  Number {meshHeight}");
                }

                if (datum > maxHRR)
                {
                    maxHRR = datum;
                }

                firePostionXYZData.Add(i);
                firePostionXYZData.Add(j);
                firePostionXYZData.Add(k);
                firePostionXYZData.Add(datum);
                // Debug.Log($"Positions {positions[0]}-{positions[1]}-{positions[2]}   datum {datum}");
                hrrCache[qFileTimeInUse].Add(firePostionXYZData);
                counter++;

            }

        

            var smokeObj = obj["smoke"];
            var smokeCounter = 0;
            foreach (var point  in smokeObj)
            {

                string position = point.ToString().Split(':')[0];
                float datum = float.Parse( point.ToString().Split(':')[1].Replace('"',' '));
                
                string[] positions = position.Replace('"',' ').Split('-');
                List<float> smokePostionXYZData = new List<float>();
                
                // Indexed position of voxel in current mesh
                float i = float.Parse(positions[0]);
                float j = float.Parse(positions[1]);
                float k = float.Parse(positions[2]);
                
                // converts relative index to global indexes
                var temp = qFilenameInUse.Split('_');
                var meshNumber = int.Parse(temp[temp.Length - 3])-1;
                
                Debug.Log($"New Time Created  {qFilenameInUse} {qFileTimeInUse}  {meshNumber}"  );
                if (meshData["multID"] != String.Empty)
                {
                    

                    var multMeshData = TerrainBuilder.multiData[meshData["multID"]];
                    var meshRow = meshNumber % (multMeshData["I_UPPER"]+1);
                    var meshCol = Math.Floor( meshNumber / (multMeshData["I_UPPER"]+1));
                    var meshHeight = Math.Floor(meshNumber / ((multMeshData["I_UPPER"]+1)* (multMeshData["K_UPPER"]+1)));

                    i += meshCol * meshData["K"];
                    j += meshHeight * meshData["J"];
                    k += meshRow * meshData["I"];
                    Debug.Log($"Row {meshRow} of {(multMeshData["I_UPPER"]+1)} Col {meshCol} of {multMeshData["K_UPPER"]+1} Height {meshHeight} of {multMeshData["J_UPPER"]+1}  Number {meshHeight}");
                }

                if (datum>maxSmokeDen)
                {
                    maxSmokeDen = datum;
                }
                
                smokePostionXYZData.Add(i);
                smokePostionXYZData.Add(j);
                smokePostionXYZData.Add(k);
                smokePostionXYZData.Add(datum);
                // Debug.Log($"Positions {positions[0]}-{positions[1]}-{positions[2]}   datum {datum}");
                smokeCache[qFileTimeInUse].Add(smokePostionXYZData);
                smokeCounter++;
                
            }
            
            //Debug.Log($"{qFilenameInUse}  Loaded HRR {counter}   smoke {smokeCounter}");
        }
        
        fireRange[1] = maxHRR;
        smokeRange[1] =  maxSmokeDen;
    }

    IEnumerator optimizedFireLoader()
    {
        var xmin = TerrainBuilder.meshData["xmin"];
        var ymin= TerrainBuilder.meshData["ymin"]; 
        var zmin= TerrainBuilder.meshData["zmin"];
        var xmax = TerrainBuilder.meshData["xmax"];
        var ymax= TerrainBuilder.meshData["ymax"]; 
        var zmax= TerrainBuilder.meshData["zmax"];
        var xcellSize = (xmax - xmin) / TerrainBuilder.meshData["I"];
        var ycellSize = (ymax - ymin) / TerrainBuilder.meshData["J"];
        var zcellSize = (zmax - zmin) / TerrainBuilder.meshData["K"];
        //Debug.Log($"Json Length {jsonFileLL.Count}");
        if (jsonFileLL.Count >= 1 && !config_script.pauseGame)
        {
            
            var worldTime = Time.time;
            
            
            qFilenameInUse = jsonFileLL.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            //Debug.Log($"Lading Voxals {worldTime} QTime {qFileTimeInUse} LL SIZE {jsonFileLL.Count}");
            if (qFileTimeInUse < worldTime )
            {
        

                jsonFileLL.RemoveFirst();
                while (qFileTimeInUse < worldTime && jsonFileLL.Count > 1)
                {
                    qFilenameInUse = jsonFileLL.First.Value;
                    qFileTimeInUse = getFileTime(qFilenameInUse);
                    jsonFileLL.RemoveFirst();
                }
                //Debug.Log($" file Length {hrrCache.Count}");
                //
                
                if (hrrCache.ContainsKey(qFileTimeInUse) && false)
                {
                    
                    //Debug.Log($" DictTime Loaded {qFileTimeInUse}   {hrrCache.ContainsKey(qFileTimeInUse)}");
                    yield return new WaitForSeconds(qFileTimeInUse - (float) worldTime + 1.0f);

                    foreach (var firePoint in hrrCache[qFileTimeInUse])
                    {
        
                        
                        
                        int k = (int) firePoint[2];
                        int j = (int) firePoint[1];
                        int i = (int) firePoint[0];
                        float datum = firePoint[3];
                        //Debug.Log($"{i}  {j}  {k}  {data[i, j, k, 4]}");
                        GameObject s = Instantiate(usedFirePrefab,
                            new Vector3((k * xcellSize) + xmin, (i * zcellSize) + zmin,
                                (j * ycellSize) + ymin), Quaternion.identity);
                        s.name = $"{qFileTimeInUse} {i}  {j}  {k}  {datum}";
                        
                        float fireValue = Mathf.InverseLerp(fireRange[0], fireRange[1], datum);
                        Color fireColor = fireGradient.Evaluate(fireValue);
                        s.GetComponent<Renderer>().material.SetColor("_Color", fireColor);
                        if (realFlames)
                        {
                            s.transform.localScale = new Vector3(xcellSize*xcellSize, zcellSize*ycellSize, ycellSize*zcellSize);
                            s.tag = "Fire3";
                        }
                        else
                        {
                            s.transform.localScale = new Vector3(xcellSize / firePrefabx,
                                zcellSize / firePrefabz, ycellSize / firePrefaby);
                            s.tag = currentFireTag;
                        }

                        
                        
                        //var qNextFilename = fileLL.ElementAt(2);
                        //var qNextFileTime = getFileTime(qNextFilename);
                        //Debug.Log($" Destroy Time {qNextFileTime - qFileTimeInUse}");
                        //Destroy(s, qNextFileTime - qFileTimeInUse);

                    }


                    



                    

                }
                if (smokeCache.ContainsKey(qFileTimeInUse))
                {
                    
                    yield return new WaitForSeconds(qFileTimeInUse - (float) worldTime + 1.0f);

                    foreach (var smokePoint in smokeCache[qFileTimeInUse])
                    {
        
                        
                        
                        int k = (int) smokePoint[2];
                        int j = (int) smokePoint[1];
                        int i = (int) smokePoint[0];
                        float datum = smokePoint[3];
                        //Debug.Log($"{i}  {j}  {k}  {data[i, j, k, 4]}");
                        GameObject s = Instantiate(usedFirePrefab,
                            new Vector3((k * xcellSize) + xmin, (i * zcellSize) + zmin,
                                (j * ycellSize) + ymin), Quaternion.identity);
                        s.name = $"{qFileTimeInUse} {i}  {j}  {k}  {datum}";
                        
                        float smokeValue = Mathf.InverseLerp(smokeRange[0], smokeRange[1], datum);
                        Color smokeColor = smokeGradient.Evaluate(smokeValue);
                        
                        s.GetComponent<Renderer>().material.SetColor("_Color", smokeColor);
                        if (realFlames)
                        {
                            s.transform.localScale = new Vector3(xcellSize*xcellSize, zcellSize*ycellSize, ycellSize*zcellSize);
                            s.tag = "Fire3";
                        }
                        else
                        {
                            s.transform.localScale = new Vector3(xcellSize / firePrefabx,
                                zcellSize / firePrefabz, ycellSize / firePrefaby);
                            s.tag = currentFireTag;
                        }

                        
                        
                        //var qNextFilename = fileLL.ElementAt(2);
                        //var qNextFileTime = getFileTime(qNextFilename);
                        //Debug.Log($" Destroy Time {qNextFileTime - qFileTimeInUse}");
                        //Destroy(s, qNextFileTime - qFileTimeInUse);

                    }


                    



                    

                }

                if (hrrCache.ContainsKey(qFileTimeInUse) || smokeCache.ContainsKey(qFileTimeInUse))
                {



                    if (currentFireTag == "Fire1")
                    {
                        currentFireTag = "Fire2";
                    }
                    else
                    {
                        currentFireTag = "Fire1";
                    }

                    GameObject[] oldFires = GameObject.FindGameObjectsWithTag(currentFireTag);
                    foreach (GameObject oldFire in oldFires)
                    {
                        Destroy(oldFire);
                    }
                }
            }
        }
    
    
    }
    
    IEnumerator fdsFireLoader() {


        if (fileLL.Count > 3 && !config_script.pauseGame && !config_script.activeSmokeStatus)
        {
            var worldTime =  Time.time;
            qFilenameInUse = fileLL.First.Value;
            qFileTimeInUse = getFileTime(qFilenameInUse);
            if (qFileTimeInUse<worldTime)
            {


                fileLL.RemoveFirst();

                while (qFileTimeInUse < worldTime && fileLL.Count > 3)
                {
                    qFilenameInUse = fileLL.First.Value;
                    qFileTimeInUse = getFileTime(qFilenameInUse);
                    fileLL.RemoveFirst();
                }

               // Debug.Log($"qFileTimeInUse < worldTime {qFileTimeInUse} < {worldTime}  Wait Time {qFileTimeInUse - (float) worldTime}");

                yield return new WaitForSeconds(qFileTimeInUse - (float) worldTime+1.0f);

                int nx;
                int ny;
                int nz;
                using (BinaryReader reader = new BinaryReader(File.Open(qFilenameInUse, FileMode.Open)))
                {

                    int _ = reader.ReadInt32();
                    nx = reader.ReadInt32();
                    ny = reader.ReadInt32();
                    nz = reader.ReadInt32();
                    //Debug.Log($"{qFilenameInUse} nx ny nz {nx}  {ny}  {nz}");
                    _ = reader.ReadInt32();
                    //
                    for (int k = 0; k < 7; k++)
                    {
                        float _f = reader.ReadSingle();
                    }

                    data = new float[nz, ny, nx, 5];
                    for (int l = 0; l < 5; l++)
                    {
                        for (int i = 0; i < nz; i++)
                        {
                            for (int j = 0; j < ny; j++)
                            {
                                for (int k = 0; k < nx; k++)
                                {
                                    data[i, j, k, l] = reader.ReadSingle();

                                }
                            }
                        }
                    }

                    var xmin = TerrainBuilder.meshData["xmin"];
                    var ymin = TerrainBuilder.meshData["ymin"];
                    var zmin = TerrainBuilder.meshData["zmin"];
                    var xmax = TerrainBuilder.meshData["xmax"];
                    var ymax = TerrainBuilder.meshData["ymax"];
                    var zmax = TerrainBuilder.meshData["zmax"];
                    var xcellSize = (xmax - xmin) / TerrainBuilder.meshData["I"];
                    var ycellSize = (ymax - ymin) / TerrainBuilder.meshData["J"];
                    var zcellSize = (zmax - zmin) / TerrainBuilder.meshData["K"];


                    for (int i = 0; i < nz; i++)
                    {
                        for (int j = 0; j < ny; j++)
                        {
                            for (int k = 0; k < nx; k++)
                            {

                                if (data[i, j, k, 4] > fireRange[0])
                                {
                                    //Debug.Log($"{i}  {j}  {k}  {data[i, j, k, 4]}");
                                    GameObject s = Instantiate(usedFirePrefab,
                                        new Vector3((k * xcellSize) + xmin, (i * zcellSize) + zmin,
                                            (j * ycellSize) + ymin), Quaternion.identity);
                                    if (!realFlames)
                                    {
                                        Color fireColor = GETColorValue(data[i, j, k, 4]);
                                        s.GetComponent<Renderer>().material.SetColor("_Color", fireColor);
                                        s.transform.localScale = new Vector3(xcellSize / firePrefabx,
                                            zcellSize / firePrefabz, ycellSize / firePrefaby);
                                    }
                                    else
                                    {
                                        Color fireColor = GETColorValue(data[i, j, k, 4]);
                                        s.GetComponent<Renderer>().material.SetColor("_Color", fireColor);
                                        s.transform.localScale = new Vector3(xcellSize, zcellSize, ycellSize);
                                    }


                                    var qNextFilename = fileLL.ElementAt(2);
                                    var qNextFileTime = getFileTime(qNextFilename);
                                    //Debug.Log($" Destroy Time {qNextFileTime - qFileTimeInUse}");
                                    Destroy(s, qNextFileTime - qFileTimeInUse+.1f);
                                
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
