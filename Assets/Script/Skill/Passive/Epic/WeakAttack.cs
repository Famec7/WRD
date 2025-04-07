using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeakAttack : PassiveAuraSkillBase
{
    protected float _originalDamage;
    
    protected override void Init()
    {
        base.Init();
        _originalDamage = weapon.Data.AttackDamage;
        weapon.AddAction(OnAttack);
    }

    private void OnAttack()
    {
        if (weapon.owner.Target is null)
        {
            return;
        }
        
        if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            TakeDamage(monster);
        }
    }

    protected virtual void TakeDamage(Monster monster)
    {
        StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

        if (mark is null)
        {
            weapon.Data.AttackDamage = _originalDamage;
        }
        else
        {
            weapon.Data.AttackDamage = _originalDamage + Data.GetValue(0);
        }
    }
}
