using System;
using System.Collections;  
using System.Collections.Generic;
using System.IO;
using UnityEngine;  
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string jsonPath;
    public static string fdsPath;
    public static string binPath;
    private bool _validInput = false;
    AsyncOperation loadingOperation;
    public GameObject slider_Object;
    private Slider progressBar;
    //input field object
    public Text tmpInputField;

    private void Start()
    {
        Debug.Log("start");
        progressBar = slider_Object.GetComponent<Slider>();
    }

    public void PlayGame()
    {
        if (_validInput)
        {
            loadingOperation= SceneManager.LoadSceneAsync("FDS_FPS");   
            slider_Object.SetActive(true);
        }
    }

    void Update()
    {
        if (loadingOperation != null)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            Debug.Log($"{Mathf.Clamp01(loadingOperation.progress / 0.9f)*100} % done ");
        }
    }

    public void FieldInput()
    {
        Debug.Log("Done With Input");

        Debug.Log(tmpInputField.text);
        jsonPath = FileExists(tmpInputField.text, "*.json");
        fdsPath = FileExists(tmpInputField.text, "*.fds");
        binPath = FileExists(tmpInputField.text, "*.bin");
        Debug.Log($"json {jsonPath} fds {fdsPath} dir {binPath}");
        if ( jsonPath != false.ToString() && fdsPath!= false.ToString() && binPath!= false.ToString())
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
        
        Debug.Log(Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly).ToString());
        if (filename == "*.bin" && Directory.GetFiles(rootPath,filename,SearchOption.TopDirectoryOnly).Length>0)
        {
            return rootPath;
        }

        Debug.Log(Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly));
        if (Directory.GetFiles(rootPath, filename, SearchOption.TopDirectoryOnly).Length == 1)
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

