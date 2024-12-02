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
        _electricShockCoroutine = CoroutineHandler.Instance.StartCoroutine(ElectricShockCoroutine());

#if STATUS_EFFECT_LOG
        Debug.Log($"{Shock Effect Applied} - Duration: {duration}");
#endif
    }

    public override void RemoveEffect()
    {
        if(target.TryGetComponent(out Status status))
        {
            status.IsElectricShock = false;
            status.DamageAmplification -= _damageAmplification;
        }
        
        if (_electricShockCoroutine != null)
        {
            CoroutineHandler.Instance.StopCoroutine(_electricShockCoroutine);
        }
        
#if STATUS_EFFECT_LOG
        Debug.Log($"{Shock Effect Removed} - Duration: {duration}");
#endif
    }

    public override void CombineEffect(StatusEffect statusEffect)
    {
        base.CombineEffect(statusEffect);
        
        if (statusEffect is ElectricShock electricShock)
        {
            _damageAmplification += electricShock._damageAmplification;
            
            if (target.TryGetComponent(out Status status))
            {
                status.DamageAmplification = _damageAmplification;
            }
        }
    }

    private IEnumerator ElectricShockCoroutine()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.IsElectricShock = true;
            status.DamageAmplification = _damageAmplification;

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