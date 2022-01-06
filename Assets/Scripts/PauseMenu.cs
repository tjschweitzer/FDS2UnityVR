using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject configData;
    private ConfigData config_script;
    public bool pauseStatus;
    public TextMeshPro timer;
    public double timeAdjustment=0.0;
    public SteamVR_Action_Boolean pauseInput;

    void Start()
    {
        config_script= configData.GetComponent<ConfigData>();
        pauseStatus = config_script.pauseGame;
    }



    // Update is called once per frame
    void Update()
    {
      

        if (pauseInput.state)
        {
            pauseStatus =!pauseStatus;
            Time.timeScale = pauseStatus ? 0.0f : 1.0f;
            
        }
    }



    private void LateUpdate()
    { 
  
       // updateTime();

    }

    private void updateTime()
    {
        if(!pauseStatus)
        {
            // Debug.Log($"WorldTime {timeAdjustment}");
            timeAdjustment += Time.deltaTime;
        }

        string roundedTime = Math.Round(timeAdjustment, 2).ToString("000.00");
        timer.text = roundedTime;
    }
}
