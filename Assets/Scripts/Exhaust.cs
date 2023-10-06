using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] initialBoostParticles;
    [SerializeField] private ParticleSystem[] continuousBoostParticles;
    void Start()
    {
        Cart parent = GetComponentInParent<Cart>();
        parent.EngineStateChangeEvent += OnEngineStateChange;
        parent.InitialBoostEvent += OnInitialBoost;
    }

    private void OnInitialBoost(object sender, Cart.InitialBoostEventArgs e)
    {
        if (e.Success)
        {
            foreach (var ps in initialBoostParticles)
            {
                ps.Play();
            }
        }
    }

    private void OnEngineStateChange(object sender, Cart.EngineStateChangeEventArgs e)
    {
        if (e.EngineState == EngineState.Boost)
        {
            foreach (var ps in continuousBoostParticles)
            {
                ps.Play();
            }
        }
    }
}
