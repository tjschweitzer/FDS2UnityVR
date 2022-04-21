using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem;
    public GameObject smvReader;
    private smvReader smvReaderScript;
    
    private Dictionary<String,dynamic> meshData;
    void Start()
    {
        smvReaderScript = smvReader.GetComponent<smvReader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void LateUpdate()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount+1];
        int l = particleSystem.GetParticles(p);
        for (int i = 0; i < p.Length; i++)
        {
            var currentPosition = p[i].position;
            p[i].velocity = smvReaderScript.getQWindVector3(p[i].velocity, currentPosition);
            
            // p[i].velocity = new Vector3(0, p[i].remainingLifetime / p[i].startLifetime * 10F, 0);

        }
  
        particleSystem.SetParticles(p, l);    
    }
}
