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
        jsonPath = FileExistsRecursive(tmpInputField.text, "sample.json");
        fdsPath = FileExistsRecursive(tmpInputField.text, "trails.fds");
        binPath = FileExistsRecursive(tmpInputField.text, "*.*");
        Debug.Log($"json {jsonPath} fds {fdsPath} dir {binPath}");
        if ( jsonPath != false.ToString() && fdsPath!= false.ToString() && binPath!= false.ToString())
        {
            // Pass Vars to next scene
            _validInput = true;
            
            Debug.Log("Input Worked");
        }
        
        
    }
    
    private string FileExistsRecursive(string rootPath, string filename)
    {

        if (filename == "*.*" && Directory.Exists(rootPath))
        {
            return rootPath;
        }
        
        if(File.Exists(Path.Combine(rootPath, filename)))
            return Path.Combine(rootPath, filename);
        

        foreach (string subDir in Directory.GetDirectories(rootPath))
        {
            if(FileExistsRecursive(subDir, filename) != false.ToString())
                return FileExistsRecursive(subDir, filename); 
        }

        return false.ToString();
    }
}

