using System;
using System.Collections;
using UnityEngine;

public class Wound : StatusEffect
{
    private Coroutine _woundCoroutine;
    
    public Wound(GameObject target, float duration = 0f) : base(target, duration)
    {
    }

    public override void ApplyEffect()
    {
        _woundCoroutine = CoroutineHandler.Instance.StartCoroutine(WoundCoroutine());
    }

    public override void RemoveEffect()
    {
        if (_woundCoroutine != null)
        {
            if(target.TryGetComponent(out Status status))
            {
                if (status.PreventWoundConsumption)
                {
                    return;
                }
                
                status.IsWound = false;
            }
            CoroutineHandler.Instance.StopCoroutine(_woundCoroutine);
        }
    }
    
    private IEnumerator WoundCoroutine()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.IsWound = true;
            
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