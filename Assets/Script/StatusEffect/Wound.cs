using System;
using System.Collections;
using UnityEngine;

public class Wound : StatusEffect
{
    public Wound(GameObject target, float duration = 0f) : base(target, duration)
    {
        ;
    }

    public override void ApplyEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.WoundStack++;
        }

        //자상 이펙트 켜주기
        monsterEffecter.SetWoundEffect(true);

#if STATUS_EFFECT_LOG
        Debug.Log("${Wound Effect Applied}");
#endif
    }

    public override void RemoveEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            if (status.PreventWoundConsumption)
            {
                return;
            }

            status.WoundStack--;
        }

        //자상 이펙트 켜주기
        if (status.WoundStack <= 0)
        {
            monsterEffecter.SetWoundEffect(false);
        }

#if STATUS_EFFECT_LOG
        Debug.Log("${Wound Effect Removed}");
#endif
    }
}