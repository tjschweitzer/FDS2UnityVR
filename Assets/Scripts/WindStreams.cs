using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityH5Loader;

public class WindStreams : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject configData;
    private ConfigData _configScript;
    public String fileDirName;

    private int _t = 0;


    private Dictionary<string, float> _fileNameDict;
    private string[] _windFiles;
    private GameObject[] _allLines;
    private int _masterCounter;
    public GameObject defaultWindObject;
    public Gradient windValueGradient;
    private Dictionary<string, Dictionary<int, Vector4[]>> _allWindParticles;
    private float _finalTime;
    private bool _showStreamLines = false;

    private float minVoxelSize;
    private Dictionary<string,dynamic> meshData = TerrainBuilder.meshData;

    void Start()
    {
        _allWindParticles = new Dictionary<string, Dictionary<int, Vector4[]>>();
        _configScript = configData.GetComponent<ConfigData>();
        Debug.Log(_configScript.getWindOption());
        if (_configScript.getWindOption() == "NoWind")
        {
            gameObject.SetActive(false);
            return;
        }

        minVoxelSize = Math.Min(meshData["xSize"], meshData["ySize"]);

        _showStreamLines = _configScript.getWindOption() == "WindVectors";
        if (_configScript.pl3dDataDir is null)
        {
            Debug.Log("Debug Path");
        }
        else
        {
            fileDirName = _configScript.windDataDir;
        }


        _windFiles = Directory.GetFiles(fileDirName, $"*.hdf5",
            SearchOption.TopDirectoryOnly);
        for (int i = 0; i < _windFiles.Length; i++)
        {

            _windFiles[i] = _windFiles[i].Replace("\\", "\\\\");
        }

        var t_windFiles = Directory.GetFiles(fileDirName, $"*.hdf5",
            SearchOption.TopDirectoryOnly);
        _masterCounter = H5Loader.LoadIntDataset(_windFiles[0], "length_of_wind_streams")[0];



        _windFiles = SortedFileArray(_windFiles);

        _fileNameDict = new Dictionary<string, float>();
        for (int i = 0; i < _windFiles.Length; i++)
        {
            _fileNameDict[_windFiles[i]] = GetFileTime(_windFiles[i]);
            readInDataHDF5(_windFiles[i]);
        }

        _finalTime = _fileNameDict[_windFiles[_windFiles.Length - 1]];
    }

    public Dictionary<int, Vector4[]> GetWindData(string filename)
    {
        return _allWindParticles[filename];
    }



    static float GetFileTime(string filename)
    {
        var breakDown = filename.Split('.')[0].Split('_');
        int fnLength = breakDown.Length;
        float hundredthsOfSecond = float.Parse(breakDown[fnLength - 1]) / 100.0f;
        float fullTime = float.Parse(breakDown[fnLength - 2]) + hundredthsOfSecond;
        return fullTime;
    }

    string[] SortedFileArray(string[] fileArray)
    {
        List<string> fileList = fileArray.ToList();
        Dictionary<float, string> fileTimeDict = new Dictionary<float, string>();

        for (int i = 0; i < fileList.Count; i++)
        {
            fileTimeDict[GetFileTime(fileList[i])] = fileList[i];
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

    void readInData(string fileName)
    {
        var time = Time.timeSinceLevelLoad;

        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {
            var maxVel = reader.ReadSingle();
            var windStreamCount = reader.ReadInt32();

            int[] windStreamPoints = new int[windStreamCount];

            for (int k = 0; k < windStreamCount; k++)
            {
                windStreamPoints[k] = reader.ReadInt32();
            }

            for (int j = 0; j < windStreamCount; j++)
            {
                var points = new Vector3[windStreamPoints[j]];

                var texture = new Texture2D(windStreamPoints[j], 1, TextureFormat.ARGB32, false);

                for (int i = 0; i < windStreamPoints[j]; i++)
                {
                    var t = reader.ReadSingle();
                    var d = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    var z = reader.ReadSingle();
                    var y = reader.ReadSingle();


                    float windValue = Mathf.InverseLerp(minVelocity, maxVel, d);
                    Color windColor = windValueGradient.Evaluate(windValue);

                    texture.SetPixel(i, 0, windColor);
                    Vector3 temp = new Vector3(x, y, z);

                    points[i] = temp;
                }

                texture.Apply();
                LineRenderer l = _allLines[j].GetComponent<LineRenderer>();
                l.startWidth = .5f;
                l.endWidth = .5f;
                l.positionCount = points.Length;
                l.SetPositions(points.ToArray());
                l.useWorldSpace = true;


                l.material = new Material(Shader.Find("Sprites/Default"));
                l.GetComponent<Renderer>().material.mainTexture = texture;
            }
        }
    }







    void loadNextWindLines(string fileName)
    {
        var currentWindData = GetWindData(fileName);
        for (int j = 0; j < currentWindData.Count; j++)
        {
            if (fileName != _windFiles[0])
            {


                Destroy(GameObject.Find($"StreakLine_{j}"));
            }

            GameObject currentStreak = new GameObject($"StreakLine_{j}");

            for (int i = 1; i < currentWindData[j].Length; i++)
            {

                var d1 = currentWindData[j][i].w;
                var x1 = currentWindData[j][i].x;
                var z1 = currentWindData[j][i].z;
                var y1 = currentWindData[j][i].y;

                var d0 = currentWindData[j][i - 1].w;
                var x0 = currentWindData[j][i - 1].x;
                var z0 = currentWindData[j][i - 1].z;
                var y0 = currentWindData[j][i - 1].y;
                var start = new Vector3(x0, y0, z0);
                var end = new Vector3(x1, y1, z1);

                float windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d1);
                Color windColorEnd = windValueGradient.Evaluate(windValue);

                windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d0);
                Color windColorStart = windValueGradient.Evaluate(windValue);

                
                var texture = new Texture2D(2, 1, TextureFormat.ARGB32, false);
                texture.SetPixel(0, 0, windColorStart);
                texture.SetPixel(1, 0, windColorEnd);
                texture.Apply();
                
                GameObject myLine = new GameObject();
                myLine.transform.SetParent(currentStreak.transform);
                myLine.transform.position = start;
                myLine.AddComponent<LineRenderer>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lr.GetComponent<Renderer>().material.mainTexture = texture;
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[]
                        { new GradientColorKey(windColorStart, 0.0f), new GradientColorKey(windColorEnd, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );
                lr.colorGradient = gradient;
                lr.startWidth = minVoxelSize;
                lr.endWidth = .01f;
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);

                //DrawLine( new Vector3(x0,y0,z0), new Vector3(x1,y1,z1),windColorStart,windColorEnd);
            }




        }
    }

    void loadNextWindVector(string fileName)
    {
        var currentWindData = GetWindData(fileName);
        for (int j = 0; j < currentWindData.Count; j++)
        {
            if (fileName != _windFiles[0])
            {


                Destroy(GameObject.Find($"StreakLine_{j}"));
            }

            GameObject currentStreak = new GameObject($"StreakLine_{j}");



            var points = new Vector3[currentWindData[j].Length];

            var texture = new Texture2D(currentWindData[j].Length, 1, TextureFormat.ARGB32, false);

            for (int i = 0; i < currentWindData[j].Length; i++)
            {
                var d = currentWindData[j][i].w;
                var x = currentWindData[j][i].x;
                var z = currentWindData[j][i].z;
                var y = currentWindData[j][i].y;


                float windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d);
                Color windColor = windValueGradient.Evaluate(windValue);

                texture.SetPixel(i, 0, windColor);
                Vector3 temp = new Vector3(x, y, z);

                points[i] = temp;
            }

            texture.Apply();
            
            currentStreak.transform.SetParent(currentStreak.transform);
            currentStreak.AddComponent<LineRenderer>();
            LineRenderer l = currentStreak.GetComponent<LineRenderer>();
            
            
            l.startWidth = minVoxelSize;
            l.endWidth = minVoxelSize;
            l.positionCount = points.Length;
            l.SetPositions(points.ToArray());
            l.useWorldSpace = true;
            
            l.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            l.GetComponent<Renderer>().material.mainTexture = texture;
            l.textureMode = LineTextureMode.DistributePerSegment;
        }
    }



private float maxVelocity = Single.NegativeInfinity;
    private float minVelocity=Single.PositiveInfinity;

    void setMaxVelovity(float value)
    {
        if (maxVelocity < value)
        {
            maxVelocity = value;
        }
        
    }
    void setMinVelovity(float value)
    {
        if (minVelocity > value && value>0.0f)
        {
            minVelocity = value;
        }
        
    }

    float getMaxVelovity()
    {
        return maxVelocity;
    }

    void readInData2Dict(string fileName)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {
            setMaxVelovity(reader.ReadSingle());
            var windStreamCount = reader.ReadInt32();

            int[] windSreamPoints = new int[windStreamCount];

            for (int k = 0; k < windStreamCount; k++)
            {
                windSreamPoints[k] = reader.ReadInt32();
            }

            _allWindParticles[fileName] = new Dictionary<int, Vector4[]>();
            for (int j = 0; j < windStreamCount; j++)
            {
                _allWindParticles[fileName][j] = new Vector4[windSreamPoints[j]];
                for (int i = 0; i < windSreamPoints[j]; i++)
                {
                    var t = reader.ReadSingle();
                    var d = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    var z = reader.ReadSingle();
                    var y = reader.ReadSingle();
                    Vector4 temp = new Vector4(x, y, z, d);
                    _allWindParticles[fileName][j][i] = temp;
                }
            }
        }
    }

    void readInDataHDF5(string filePath)
    {
     //   Debug.Log( H5Loader.LoadIntDataset(filePath, "maxValue"));
        int[] windStreamPoints = H5Loader.LoadIntDataset(filePath, "length_of_wind_streams");

        _allWindParticles[filePath] = new Dictionary<int, Vector4[]>();
            for (int j = 0; j < windStreamPoints.Length; j++)
            {
                _allWindParticles[filePath][j] = new Vector4[windStreamPoints[j]];
                
                float[,] currentWindPoints = H5Loader.Load2dFloatDataset(filePath, $"windStream_{j+1}");
                for (int i = 0; i < windStreamPoints[j]; i++)
                {
                    var t = currentWindPoints[i, 0];
                    var d = currentWindPoints[i, 1];
                    
                    setMaxVelovity( d);
                    setMinVelovity( d);
                    var x = currentWindPoints[i, 2];
                    var z = currentWindPoints[i, 3];
                    var y = currentWindPoints[i, 4];

                    Vector4 temp = new Vector4(x, y, z, d);
                    _allWindParticles[filePath][j][i] = temp;
                }
            }
        
    }


    // Update is called once per frame
    void Update()
    {

        if (_t > _windFiles.Length-1)
            
            return;
        if (Time.timeSinceLevelLoad > _fileNameDict[_windFiles[_t]] &&
            Time.timeSinceLevelLoad < _fileNameDict[_windFiles[_t + 1]] )
        {
            if (_showStreamLines)
            {
                loadNextWindLines(_windFiles[_t]);
            }
            else
            {
                loadNextWindVector(_windFiles[_t]);
            }

            _t += 1;
        }
    }
}