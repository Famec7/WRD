using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Lancer : PassiveAuraSkillBase
{
    protected override void Init()
    {
        base.Init();

        weapon.AddAction(OnAttack);
    }

    protected void OnAttack()
    {
        if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            StatusEffect woundEffect = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

            if (woundEffect != null)
            {
                weapon.GetActiveSkill().CurrentCoolTime -= Data.GetValue(0);
            }
        }
    }
}
