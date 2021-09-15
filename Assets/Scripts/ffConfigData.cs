using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ffConfigData : MonoBehaviour
{
    
    public string fileName =  @"/home/kl3pt0/Work/fast_fuels/treelist.json";
    public string pl3dDataDir = @"/home/kl3pt0/FastFuels_Steam_VR/TestData";
    public bool smoothTerrain=false;
    public bool realisticFlames = false;

    public string[] terrainLabels = new[] {"wet vegetation"};
    public string[]obstacleLabels = new[] {"Urban","Snow-Ice","Agriculture","Water","Barren","NA"};

}
