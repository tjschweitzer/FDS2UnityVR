using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class PickupHand : MonoBehaviour
{
    public float distToPickup = .2f;
    private bool handClosed=false;
    public LayerMask pickupLayer;

    public SteamVR_Input_Sources handSource = SteamVR_Input_Sources.RightHand;

    private Rigidbody holdingTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
      
    }
}
