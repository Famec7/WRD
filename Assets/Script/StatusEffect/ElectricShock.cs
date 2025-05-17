using System;
using System.Collections;
using UnityEngine;

public class ElectricShock : StatusEffect
{
    private Coroutine _electricShockCoroutine;
    private float _damageAmplification;

    public ElectricShock(GameObject target, float damageAmplification, float duration = 0) : base(target, duration)
    {
        _damageAmplification = damageAmplification;
    }

    public override void ApplyEffect()
    {
        _electricShockCoroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(ElectricShockCoroutine());

#if STATUS_EFFECT_LOG
        Debug.Log($"{Shock Effect Applied} - Duration: {duration}");
#endif
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.ElectricShockStack--;
            status.DamageAmplification -= _damageAmplification;
        }
        
        if (_electricShockCoroutine != null)
        {
            target.GetComponent<MonoBehaviour>().StopCoroutine(_electricShockCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log($"{Shock Effect Removed} - Duration: {duration}");
#endif
        
        StatusEffectManager.Instance.RemoveValue(status, this);
    }

    private IEnumerator ElectricShockCoroutine()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.ElectricShockStack++;
            status.DamageAmplification += _damageAmplification;

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
}