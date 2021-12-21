using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public SteamVR_Action_Boolean input;
    public float speed = 10;

        
    public GameObject configData;
    private ConfigData config_script;

    
    void Start()
    {
        config_script= configData.GetComponent<ConfigData>();

        
    }

    // Update is called once per frame
    void Update()
    {


        if (input.state)
        {
            
            Vector3 direction = Player.instance.leftHand.transform.forward;
        
            var delta = Time.timeScale==0 ? 0.1f : Time.deltaTime;

            transform.position += speed * delta * direction;

            
        }
        
    }

}
