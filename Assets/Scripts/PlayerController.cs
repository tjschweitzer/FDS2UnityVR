using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public SteamVR_Action_Boolean input;
    public SteamVR_Action_Boolean pauseInput;
    public float speed = 10;
  
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {



        if (input.state)
        {

            Vector3 direction = Player.instance.leftHand.transform.forward;

            var delta = pauseInput.state ? 0.1f : Time.deltaTime;

            transform.position += speed * delta * direction;

            
        }
        
    }

}
