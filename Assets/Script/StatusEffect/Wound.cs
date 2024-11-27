using System;
using System.Collections;
using UnityEngine;

public class Wound : StatusEffect
{
    private Coroutine _woundCoroutine;
    
    public Wound(GameObject target, float duration = 0f) : base(target, duration)
    {
        //Debug.Log("자상 이펙트 적용");
    }

    public override void ApplyEffect()
    {
        _woundCoroutine = CoroutineHandler.Instance.StartCoroutine(WoundCoroutine());

        //자상 이펙트 켜주기
        monsterEffecter.SetWoundEffect(true);
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

        //자상 이펙트 켜주기
        monsterEffecter.SetWoundEffect(false);
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