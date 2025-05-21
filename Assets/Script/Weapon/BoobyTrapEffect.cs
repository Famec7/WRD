using UnityEngine;

public class BoobyTrapEffect : ParticleEffect
{
    [SerializeField] private GameObject particleEffectTrap;
    
    public GameObject ParticleEffectTrap => particleEffectTrap;
}