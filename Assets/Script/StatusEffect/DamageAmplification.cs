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
            PlayEffect(status.DamageAmplification);
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
            PlayEffect(status.DamageAmplification);


            if (Math.Abs(duration - 0f) > 0.01f)
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

    /// <summary>
    /// 받피증 연출 제어 함수(이펙트 제어를 Status에서 Getter Setter로 할 수 있긴 한데... 우선 시간 걸릴 거 같아서 보류)
    /// </summary>
    /// <param name="damageAmplification"></param>
    void PlayEffect(float damageAmplification)
    {
        if(damageAmplification > 0)
        {
            monsterEffecter.SetDebuffEffect(true);
        }
        else
        {
            monsterEffecter.SetDebuffEffect(false);
        }
    }
}