using System;
using System.Collections;
using UnityEngine;

public class HitEffect : EffectBase
{
    [SerializeField]
    private string effectName;
    
    private ParticleSystem _hitEffect;
    
    protected override void Init()
    {
        _hitEffect = GetComponent<ParticleSystem>();
    }

    public override void PlayEffect()
    {
        _hitEffect.Play();
    }

    public override void StopEffect()
    {
        _hitEffect.Stop();
        EffectManager.Instance.ReturnEffectToPool(this, effectName);
    }

    private void OnParticleSystemStopped()
    {
        StopEffect();
    }
}