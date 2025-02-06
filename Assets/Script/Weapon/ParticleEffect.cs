using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : EffectBase
{
    [SerializeField]
    private string effectName;
    
    private ParticleSystem _hitEffect;
    
    private Vector3 _originScale;
    
    protected override void Init()
    {
        _originScale = transform.localScale;
        
        _hitEffect = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = _hitEffect.main;
        main.stopAction = ParticleSystemStopAction.Callback;
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
    
    public void ResetScale()
    {
        SetScale(_originScale);
    }

    private void OnParticleSystemStopped()
    {
        StopEffect();
    }
}