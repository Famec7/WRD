using System;
using System.Collections;
using UnityEngine;

public class HitEffect : EffectBase
{
    [SerializeField]
    private string effectName;
    
    private ParticleSystem _hitEffect;
    
    private Vector3 _originScale;
    
    protected override void Init()
    {
        _hitEffect = GetComponent<ParticleSystem>();
        _originScale = transform.localScale;
    }

    public override void PlayEffect()
    {
        _hitEffect.Play();
    }

    public override void StopEffect()
    {
        _hitEffect.Stop();
        EffectManager.Instance.ReturnEffectToPool(this, effectName);
        
        SetScale(_originScale);
    }

    private void OnParticleSystemStopped()
    {
        StopEffect();
    }
}