using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireLifeAlpha : MonoBehaviour
{
    
    // Start is called before the first frame update
    public ParticleSystem particleSystem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void fLateUpdate()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount + 1];
        int l = particleSystem.GetParticles(p);
        for (int i = 0; i < p.Length; i++)
        {
            var currentColor =  p[i].startColor;
            var lifeTime = p[i].remainingLifetime;
            
            if (lifeTime>0.5f)
            {

               // p[i].startColor = new Color32(currentColor.r,currentColor.g,currentColor.b, (byte)(currentColor.a *0.75f));
                // Debug.Log($"Lowered Alpha {p[i].startLifetime} {lifeTime}");
            } 
            // p[i].velocity = new Vector3(0, p[i].remainingLifetime / p[i].startLifetime * 10F, 0);

        }
 
        particleSystem.SetParticles(p, l);
    }

}
