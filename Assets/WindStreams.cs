using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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
    private Dictionary<string, Dictionary<string, Vector4[]>> _allWindParticles;
    private float _finalTime;
    private bool _showStreamLines = true;
    

    
    void Start()
    {
        _allWindParticles = new Dictionary<string, Dictionary<string, Vector4[]>>();
        _configScript = configData.GetComponent<ConfigData>();
        if (_configScript.getWindOption()=="")
        {
            gameObject.SetActive(true);
        }

        _showStreamLines = _configScript.getWindOption() == "WindVectors";
        if (_configScript.pl3dDataDir is null)
        {
            Debug.Log("Debug Path");
        }
        else
        {
           // fileDirName = @$"{_configScript.pl3dDataDir}\wind\";
        }

        _windFiles = Directory.GetFiles(fileDirName, $"*.bin",
            SearchOption.TopDirectoryOnly);

        using (BinaryReader reader = new BinaryReader(File.Open(_windFiles[0], FileMode.Open)))
        {
            var blank = reader.ReadSingle();
            _masterCounter = reader.ReadInt32();

            _allLines = new GameObject[_masterCounter];
            for (int i = 0; i < _masterCounter; i++)
            {
                _allLines[i] = new GameObject();
                
                GameObject s = Instantiate(defaultWindObject,
                    new Vector3(0, 0, 0), Quaternion.identity);
                _allLines[i]=s;
            }
        }

        _windFiles = SortedFileArray(_windFiles);

        _fileNameDict = new Dictionary<string, float>();
        for (int i = 0; i < _windFiles.Length; i++)
        {
            _fileNameDict[_windFiles[i]] = GetFileTime(_windFiles[i]);
            readInData2Dict(_windFiles[i]);
        }

        _finalTime = _fileNameDict[_windFiles[_windFiles.Length - 1]];
    }

    public Dictionary<string, Vector4[]> GetWindData(string filename)
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


                    float windValue = Mathf.InverseLerp(0.0f, maxVel, d);
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

    

    void DrawLine(Vector3 start, Vector3 end, Color colorstart, Color colorEnd)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(colorstart, 0.0f), new GradientColorKey(colorstart, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lr.colorGradient = gradient;
        lr.startWidth = .5f;
        lr.endWidth = .01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        if (_t< _windFiles.Length-2)
        {
            Destroy(myLine,_fileNameDict[_windFiles[_t+1]]-Time.timeSinceLevelLoad);
        }

    }

    

    void loadNextWindLines(string fileName)
    {
        var currentWindData = GetWindData(fileName);
        for (int j = 0; j < currentWindData.Count; j++)
        {
            var points = new Vector3[currentWindData[j.ToString()].Length];

            var texture = new Texture2D(currentWindData[j.ToString()].Length, 1, TextureFormat.ARGB32, false);
            

            for (int i = 1; i <currentWindData[j.ToString()].Length; i++)
            {
                
                var d1 = currentWindData[j.ToString()][i].w;
                var x1 = currentWindData[j.ToString()][i].x;
                var z1 = currentWindData[j.ToString()][i].z;
                var y1 = currentWindData[j.ToString()][i].y;
                
                var d0 = currentWindData[j.ToString()][i-1].w;
                var x0 = currentWindData[j.ToString()][i-1].x;
                var z0 = currentWindData[j.ToString()][i-1].z;
                var y0 = currentWindData[j.ToString()][i-1].y;


                float windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d1);
                Color windColorEnd = windValueGradient.Evaluate(windValue);

                windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d0);
                Color windColorStart = windValueGradient.Evaluate(windValue);
                
                

                DrawLine( new Vector3(x0,y0,z0), new Vector3(x1,y1,z1),windColorStart,windColorEnd);
            }




        }
    }
    void loadNextWindVector(string fileName)
    {
        Debug.Log("WindVector Arrows");
        var currentWindData = GetWindData(fileName);
        for (int j = 0; j < currentWindData.Count; j++)
        {
            var points = new Vector3[currentWindData[j.ToString()].Length];

            var texture = new Texture2D(currentWindData[j.ToString()].Length, 1, TextureFormat.ARGB32, false);

            for (int i = 0; i < currentWindData[j.ToString()].Length; i++)
            {
                var d = currentWindData[j.ToString()][i].w;
                var x = currentWindData[j.ToString()][i].x;
                var z = currentWindData[j.ToString()][i].z;
                var y = currentWindData[j.ToString()][i].y;


                float windValue = Mathf.InverseLerp(0.0f, getMaxVelovity(), d);
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

    private float maxVelocity = 0.0f;

    void setMaxVelovity(float value)
    {
        if (maxVelocity < value)
        {
            maxVelocity = value;
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

            _allWindParticles[fileName] = new Dictionary<string, Vector4[]>();
            for (int j = 0; j < windStreamCount; j++)
            {
                _allWindParticles[fileName][j.ToString()] = new Vector4[windSreamPoints[j]];
                for (int i = 0; i < windSreamPoints[j]; i++)
                {
                    var t = reader.ReadSingle();
                    var d = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    var z = reader.ReadSingle();
                    var y = reader.ReadSingle();
                    Vector4 temp = new Vector4(x, y, z, d);
                    _allWindParticles[fileName][j.ToString()][i] = temp;
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_t > _windFiles.Length-1)
            return;
        if (Time.timeSinceLevelLoad > _fileNameDict[_windFiles[_t]] &&
            Time.timeSinceLevelLoad <= _finalTime&&
            Time.timeSinceLevelLoad < _fileNameDict[_windFiles[_t + 1]] )
        {
            if (_showStreamLines)
            {
                loadNextWindVector(_windFiles[_t]);
            }
            else
            {
                loadNextWindLines(_windFiles[_t]);
            }

            _t += 1;
        }
    }
}