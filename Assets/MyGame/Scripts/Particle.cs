using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] ParticlePool _particlePool = null;

    public void AssignPool(ParticlePool particlePool)
    {        
        _particlePool = particlePool;
    }
}
