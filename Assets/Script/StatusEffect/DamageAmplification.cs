using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAmplification : StatusEffect
{
    private Coroutine _damageAmplificationCoroutine;
    private float _amplificationRate;
    
    public DamageAmplification(GameObject target, float amplificationRate, float duration = 0) : base(target, duration)
    {
        _amplificationRate = amplificationRate;
    }

    public override void ApplyEffect()
    {
        _damageAmplificationCoroutine = CoroutineHandler.Instance.StartCoroutine(DamageAmplificationCoroutine());

#if STATUS_EFFECT_LOG
        Debug.Log($"{nameof(DamageAmplification)} Effect Applied - Amplification Rate: {_amplificationRate}");
#endif
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.DamageAmplification -= _amplificationRate;
        }
        
        if (_damageAmplificationCoroutine != null)
        {
            CoroutineHandler.Instance.StopCoroutine(_damageAmplificationCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log($"{nameof(DamageAmplification)} Effect Removed - Amplification Rate: {_amplificationRate}");
#endif
    }

    private IEnumerator DamageAmplificationCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.DamageAmplification += _amplificationRate;
            
            if(Math.Abs(duration - 0f) > 0.01f)
            {
                yield return waitTime;
                
                RemoveEffect();
            }
            else
            {
                yield return null;
            }
        }
    }
}