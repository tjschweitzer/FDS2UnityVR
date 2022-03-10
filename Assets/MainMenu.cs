using System;
using System.Collections;  
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;  
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string JsonPath;
    public static string FdsPath;
    public static string BinPath;
    public static string FireSmokeOption;
    public static string WindOption;
    public static bool TreesActive;
    


    public static string WindPath;
    
    public ToggleGroup SmokeFireGroup;
    public ToggleGroup WindTypeGroup;
    public Toggle TreeToggle;
    
    private bool _validInput = false;
    AsyncOperation _loadingOperation;
    [FormerlySerializedAs("slider_Object")] public GameObject sliderObject;
    private Slider _progressBar;
    //input field object
    public Text tmpInputField;
    public TextMeshProUGUI  keyboardInput;
    
    private void Start()
    {
        Debug.Log("start");
        _progressBar = sliderObject.GetComponent<Slider>();
    }

    public void FireSmokeToggleChange()
    {
        var fireSmokeList = SmokeFireGroup.ActiveToggles().ToList();
        if (fireSmokeList.Count !=1)
        {
            Debug.Log("Incorrect number of fireSmoke options selected");
            return;
        }
        FireSmokeOption  = fireSmokeList[0].name;
        Debug.Log($"Fire Option Selected { FireSmokeOption }");
    }
    public void TreeToggleChange()
    {
        TreesActive = TreeToggle.isOn;

    }
    public void WindToggleChange()
    {
        var windList = WindTypeGroup.ActiveToggles().ToList();
        
        if (windList.Count ==0)
        {
            WindOption = "";
            return;
        }
        if (windList.Count >1)
        {
            Debug.Log("Incorrect number of wind options selected");
            return;
        }

        WindOption = windList[0].name;

    }

     
    public void PlayGame()
    {
        if (_validInput)
        {
            _loadingOperation= SceneManager.LoadSceneAsync("FDS_FPS");   
            sliderObject.SetActive(true);
        }
    }

    void Update()
    {
        if (_loadingOperation != null)
        {
            _progressBar.value = Mathf.Clamp01(_loadingOperation.progress / 0.9f);
            Debug.Log($"{Mathf.Clamp01(_loadingOperation.progress / 0.9f)*100} % done ");
        }
    }

    public void UpdateDirTexField()
    {
        tmpInputField.text = keyboardInput.text;
    }


    public void FieldInput()
    {
        Debug.Log("Done With Input");

        Debug.Log(tmpInputField.text);
        JsonPath = FileExists(tmpInputField.text, "*.json");
        FdsPath = FileExists(tmpInputField.text, "*.fds");
        BinPath = FileExists(tmpInputField.text, "*.bin");
        WindPath = FileExists(tmpInputField.text, "*.binwind");
        Debug.Log($"json {JsonPath} fds {FdsPath} dir {BinPath} wind {WindPath}");
        if ( JsonPath != false.ToString() && FdsPath!= false.ToString() && BinPath!= false.ToString()&& WindPath!= false.ToString())
        {
            // Pass Vars to next scene
            _validInput = true;
            
            Debug.Log("Input Worked");
        }
        
        
    }
    
    private string FileExists(string rootPath, string filename)
    {

        if ( !Directory.Exists(rootPath))
        {
            return false.ToString();
        }

        foreach (var variable in Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly))
        {
            Debug.Log(variable);   
        }
        if (filename == "*.bin" && Directory.GetFiles(rootPath,filename,SearchOption.TopDirectoryOnly).Length>0)
        {
            return rootPath;
        }

        if (Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly).Length >= 1)
            return Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly)[0];
        

        foreach (string subDir in Directory.GetDirectories(rootPath))
        {
            var result = FileExists(subDir, filename);
            if (result != false.ToString())
                return result;
        }

        return false.ToString();
    }
}

