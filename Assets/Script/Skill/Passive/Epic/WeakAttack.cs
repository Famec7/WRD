using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeakAttack : PassiveAuraSkillBase
{
    private float _originalDamage;
    
    protected override void Init()
    {
        base.Init();
        weapon.AddAction(OnAttack);
    }

    private void OnAttack()
    {
        if (weapon.owner.Target is null)
        {
            return;
        }
        
        if (weapon.owner.Target.TryGetComponent(out Status status))
        {
            StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(status, typeof(Mark));
            
            if (mark is null)
            {
                weapon.Data.AttackDamage = _originalDamage + Data.GetValue(0);
            }
            else
            {
                weapon.Data.AttackDamage = _originalDamage;
            }
        }
    }
}
