using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class tduController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject configData;
    private ConfigData config_script;
    public string deviceIDFilter = "RADIATIVE HEAT FLUX GAS";
    public List<float> timeDevc;
    public List<int> devcIndexNeeded;
    private List<string> header1Values=null;
    private List<string> header2Values=null;
    public float[,] valuesDevcArray;
    public List<List<float>> valuesDevc;
    public GameObject devcPreFab;
    public Dictionary<String,  List<String>> DevcDictionary;
    private List<String> devcList;
    private bool devcFlag = false;

    public GameObject pauseMenu;
    private PauseMenu pauseScript;
    void Start()
    {
        config_script= configData.GetComponent<ConfigData>();

        string fileName = config_script.fileName;
        
        pauseScript= pauseMenu.GetComponent<PauseMenu>();

        var time = pauseScript.timeAdjustment;
        
        
        DevcDictionary = new Dictionary<string, List<string>>();
        locateDEVC(fileName);
        
        readinDEVCCSV();
        if (devcFlag)
        {


            CalculateTIE();
            TrapTIEToTDU();
        }
    }

    private void readinDEVCCSV()
    {
        string plot3dDir = config_script.pl3dDataDir;
        string[] csvFiles =  Directory.GetFiles(plot3dDir,"*devc.csv", SearchOption.AllDirectories);
        if (csvFiles.Length>1)
        {
            Debug.Log("More then one devc.csv file found");
            return;
        }
        if (csvFiles.Length<1)
        {
            Debug.Log("No  devc.csv file found");
            return;
        }

        devcFlag = true;

        string devcFilename = csvFiles[0];
        
        var fileContent = File.ReadAllLines(devcFilename);

        List<string> contentAllRows= null;
        if (fileContent !=null && fileContent.Any())
        {
            header1Values = fileContent.First().Split(',').Skip(1).ToList();
            header1Values.ForEach(h => h = h.Trim());
            header2Values = fileContent[1].Split(',').Skip(1).ToList();
            header2Values.ForEach(h => h = h.Trim());
            contentAllRows = fileContent.Skip(2).ToList();

            timeDevc = new List<float>();
            valuesDevc = new List<List<float>>();
            
            getDeviceIndexs(header1Values,header2Values);            
            //
            foreach (string currentRow in contentAllRows)

            {
                //Debug.Log(currentRow);
                String[] currentRowString = currentRow.Split(',');
                List<float> currentRowFloat = Array.ConvertAll(currentRowString, float.Parse).ToList();
                timeDevc.Add(currentRowFloat[0]);
                valuesDevc.Add(currentRowFloat.Skip(1).ToList());
            }
            
            
        }
        
        int[] s_d = new int[]  { valuesDevc.Count, valuesDevc[0].Count};


        valuesDevcArray = new float[s_d[0], s_d[1]];

        for (int i = 0; i < s_d[0]; i++)
        {
            for (int j = 0; j < s_d[1]; j++)
            {
                valuesDevcArray[i, j] = valuesDevc[i].ToArray()[j];
            }
        }

    }

    private void getDeviceIndexs(List<string>h1,List<string>h2)
    {
        devcIndexNeeded = new List<int>();
        for (int i = 1; i < h1.Count; i++)
        {
            if (devcList[i] == deviceIDFilter)
            {
                devcIndexNeeded.Add(i);
            }
        }


    }
    
    private void locateDEVC(string fileName )
    {
        FileInfo file = new FileInfo( fileName);
        var f = file.OpenText();
        string cur_line;
        devcList = new List<string>();
        while ((cur_line = f.ReadLine()) != null)
        {
            if (cur_line.Trim().StartsWith("&DEVC"))
            {
                string pointID = cur_line.Split(new string[] {"ID='"}, StringSplitOptions.None)[1].Split('\'')[0];
                string pointQuantity = cur_line.Split(new string[] {"QUANTITY='"}, StringSplitOptions.None)[1].Split('\'')[0];
                string[] cords = new string[3];
                cords = cur_line.Split(new string[] {"XYZ="}, StringSplitOptions.None)[1].Split(',').Take(3).ToArray();
                devcList.Add(pointQuantity);
                if (pointQuantity == deviceIDFilter)
                {


                    GameObject devcObj = Instantiate(devcPreFab,
                        new Vector3(float.Parse(cords[0]), float.Parse(cords[2]), float.Parse(cords[1])),
                        Quaternion.identity);
                    devcObj.name = pointID + "$" + pointQuantity;
                }

                if (DevcDictionary.ContainsKey(pointID))
                {
                    DevcDictionary[pointID].Add(pointQuantity);
                }
                else
                {
                    DevcDictionary[pointID] = new List<string>();
                    DevcDictionary[pointID].Add(pointQuantity);
                }
            

            }
        }
        

    }

    public float hrrMax = 500;

    Color GETColorValue(float dataValue)
    {

        if (dataValue>3500f)
        {
            return new Color(0f,0f,0f, 1f);
        }
        if (dataValue>2300f)
        {
            return new Color(1f,0f,0f, 1f);
        }
        
        if (dataValue>1050f)
        {
            return new Color(1f, .353f,0f, 1f);
        }
        if (dataValue>255f)
        {
            return new Color(1f,1f,0f, 1f);
        }
        
        return new Color(0f,1f,0.0f, 0.1f);

    }

    public float[,] TIEArray;
    //Total incident energy in kw/m at a set time ( via RowNumber)
    private void CalculateTIE()
    {
         //  valuesDevc
         
         
    
        int[] s_d =   { valuesDevc.Count, valuesDevc[0].Count};
    
        //determine number of devc's (there are 6 cols per devc, plus an
        // initial time column
        int nd = (int) (s_d[1] / 6);
    
        // now, for each devc, get the temp column (time col is constant at col 1).
 
        int[] tcols = new int[nd]; // temp in c
        int[] ucols = new int[nd]; // wind u
        int[] vcols = new int[nd]; // wind v
        int[] wcols = new int[nd]; // wind w
        TIEArray = new float[timeDevc.Count,nd];
                      //     FEDcols = 6:6:s_d(2);% Fractional Effective Dose
        int[] radcols = new int[nd];// kw/m^2
        for (int i = 0; i < nd; i++)
        {
            tcols[i] = 0+i*6; //temp in c
            ucols[i] = 1+i*6; // wind u
            vcols[i] = 2+i*6; // wind v
            wcols[i] = 3+i*6; // wind w
            radcols[i] = 5+i*6;// kw/m^2
        }
        // RADIATIVE HEAT FLUX GAS
        //RadHeatFluxGas = d(PeakStartTime_ind:PeakEndTime_ind,radcols);
        for (int j = 0; j < timeDevc.Count; j++)
        {


            for (int i = 0; i < nd; i++)
            {


                var RadHeatFluxGas = valuesDevcArray[j, radcols[i]];

                // step 1 calc vel from eqn1
                // vel = (d(PeakStartTime_ind:PeakEndTime_ind,ucols).^2+d(PeakStartTime_ind:PeakEndTime_ind,vcols).^2+d(PeakStartTime_ind:PeakEndTime_ind,wcols).^2).^0.5;
                var vel = Math.Pow(
                    Math.Pow(valuesDevcArray[j, ucols[i]], 2) +
                    Math.Pow(valuesDevcArray[j, vcols[i]], 2) +
                    Math.Pow(valuesDevcArray[j, vcols[i]], 2), .5);

                // step 2 calc density at air temp

                // AirTemp = d(PeakStartTime_ind:PeakEndTime_ind,tcols)+ 273.15; %  deg C, expressed here in Kelvin
                var AirTemp = valuesDevcArray[j, tcols[i]] + 273.15; //deg C, expressed here in Kelvin

                var GasConst = 287.05; // ideal gas constant J/kg-K
                var ConvHg_to_NM2 = 2.953e-4; // conversion from pressure in Hg to N/m@
                var background_pressure = 101.325; // kPa, default for FDS (= 101325 Pa).
                var background_pressure_mmHg =
                    background_pressure * (760.0 / 101.325); // convert pressure from kPa to mmHg
                var background_pressure_Hg = background_pressure_mmHg / 25.4; // convert pressure from mmHg to inches Hg

                // s_AirTemp = size(AirTemp);
                // the line below was for full range of timesteps in d
                //air_Dens = (ones(s_d(1),nd).*background_pressure_Hg)./(AirTemp.*GasConst*ConvHg_to_NM2);
                // this one is for restricted time range in AirTemp etc.
                var air_Dens = (background_pressure_Hg) / (AirTemp * GasConst * ConvHg_to_NM2);

                // step 3 calc film temp as 290/air_temp
                // 2/7/2012 -- corrected as follows: FilmTemp = average of surface and
                // air temperatures, where surf temp is human skin temp (330 deg K).
                var FilmTemp = (AirTemp + 308.65) / 2; // 308.65 K = 96degrees F or 35.555 C. Film temp avgs.    
                // step 4 evaluate all properties at film temp
                var D = 0.1; // characteristic diameter 0.1m
                //quantities needed to calculate convective heat
                // k: thermal conductivity of air. evaluated at film temperature (FilmTemp)
                var k = (((Math.Pow(FilmTemp, 4)) * 1.9352e-11) - (Math.Pow(FilmTemp, 3) * 3.2146e-8) -
                    (Math.Pow(FilmTemp, 2) * 1.4887e-5) + (FilmTemp * 9.4233e-2) + 8.9802) / 1000;

                // specific hear of air at film temperature
                // cp = ((FilmTemp.^4).*2.4424e-13) -  ((FilmTemp.^3).*9.8393e-10) + ((FilmTemp.^2).*1.3598e-6) - (FilmTemp.*5.536e-4) + 1.0748;
                var cp = (Math.Pow(FilmTemp, 4) * 2.4424e-13) - (Math.Pow(FilmTemp, 3) * 9.8393e-10) +
                    (Math.Pow(FilmTemp, 2) * 1.3598e-6) - (FilmTemp * 5.536e-4) + 1.0748;

                // kinematic viscosity of air at film temperature
                var v = ((Math.Pow(FilmTemp, 4) * 1.5153e-11) - (Math.Pow(FilmTemp, 3) * 6.7113e-8) +
                    (Math.Pow(FilmTemp, 2) * 1.6123e-4) + (FilmTemp * 1.3507e-2) - 0.91155) / 1000000;

                // Debug.Log($" AirTemp {AirTemp} FilmTemp {FilmTemp}  K {k} CP {cp} V {v}");

                // step 5 calc Re, Pr and h
                // Reynolds number
                var Re = (vel * D) / v; // this is many rows by nd columns (e.g. 1001 x 42), Reynolds number
                var alpha = k / (air_Dens * cp);
                // Prandtl number
                var Pr = v / alpha;

                // Heat transfer coefficient

                // this is complicated so I am going to break it up.

                var term1 = (0.62 * Math.Pow(Re, 0.5) * Math.Pow(Pr, (float) 1 / 3));
                var term2 = (Math.Pow(1 + Math.Pow(0.4 / Pr, (float) 2 / 3), .25));
                var term3 = (Math.Pow(1 + Math.Pow(Re / 282000, .625), .8));

                var hbar = (k / D) * (0.3 + ((term1) / (term2)) * (term3));


                //step 6, calc q and evaluate time to burn injury from fig 2
                // Calc A, surface area of arm. Assume a cylinder 0.1 m in diam and 0.6m
                // in length for now.
                var arm_length = 0.65; // m % was 0.8m (my arm is less than 0.8m so that is probably too long)
                var arm_diam = 0.1; //m % was 0.1

                var A = arm_length * arm_diam * Math.PI; // surface area of cylinder is length * circumference

                // Calculate heat transfer to subject's arm.
                // Here, we want the temperature change to always be positive.
                var SkinTemp = 308.65; // 96 F in deg K.

                var q = hbar * A * (AirTemp - 272.15); // drop to 272.15 so always positive, across all cases was 290.0
                // 13-May-2012 16:16:34
                // calc dosage based on email from Bret 4/18/2012
                var TIE = Math.Pow(q + RadHeatFluxGas, (float) 4 / 3); // total incident energy in kw/m
               // Debug.Log($"TIE  {TIE} ,  row {j} ,i {i}");

                // calculate time integrated values
                //var intg_q = TrapezRule(x => x * x, 1, 2, i));
                ////(PeakStartTime_ind:PeakEndTime_ind,1),q);% convective heat flux (integrated)
                TIEArray[j,i] = (float) TIE;
            }
        }

    }
    
    // Trapezoidal numerical integration
    // Func : Change to Grab the TIE value 
    // A: Start Time
    
    // B: End time 

    public float[,] TduArray;
    private void TrapTIEToTDU()
    {
        int[] s_d =   { valuesDevc.Count, valuesDevc[0].Count};
        int nd = (int) (s_d[1] / 6);

        TduArray = new float[timeDevc.Count,nd];
        
        for (int i = 0; i < nd; i++)
        {
            TduArray[0, i] = 0.0f;
        }
        for (int i = 1; i < timeDevc.Count; i++)
        {
            float deltaTime = timeDevc[i] - timeDevc[i - 1]; 
            for (int j = 0; j < nd; j++)
            {
                TduArray[i,j] =TIEArray[i, j] * deltaTime+TduArray[i-1,j];
            }
        }


    }

    private int counter = 0;
    void Update()
    {
        if (devcFlag)
        {
            var worldTime = pauseScript.timeAdjustment;
            Debug.Log($" timeDevc[counter] <= worldTime  {timeDevc[counter]} <= {worldTime}  ");
            if (counter < timeDevc.Count - 1 && timeDevc[counter] <= worldTime)
            {
                counter++;

               // Debug.Log((timeDevc[counter] - wordTime)+"TIme difference "+ wordTime);
                while (counter < timeDevc.Count - 1 &&timeDevc[counter] - worldTime < 0.0f)
                {
                    counter++;
                  //  Debug.Log(timeDevc[counter] + " to world  " + wordTime);
                }
                // Debug.Log($"TIE Value { TIEArray[counter,19]}");
                for (int index = 0; index < devcIndexNeeded.Count; index++)
                
                {
                    string devcName = header2Values[devcIndexNeeded[index]].Replace("\"", "") + "$" + deviceIDFilter;
                    GameObject currentDevice = GameObject.Find(devcName);

                    var tempy = TduArray[counter,index];

                    if (tempy < 1.0f)
                    {
                        tempy = 1.0f;
                    }

                    Color devcColor = GETColorValue(tempy);
                    currentDevice.GetComponent<Renderer>().material.SetColor("_Color", devcColor);
                }

            }
            else
            {
                //
                Debug.Log("DEVC Frame Skipped");
            }
        }
    }
}
