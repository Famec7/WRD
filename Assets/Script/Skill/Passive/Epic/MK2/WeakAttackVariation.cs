using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeakAttackVariation : WeakAttack
{
    protected override void TakeDamage(Monster monster)
    {

        StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

        if (mark != null)
        {
            StatusEffect amplification = new DamageAmplification(monster.gameObject, Data.GetValue(1), Data.GetValue(0));
            StatusEffectManager.Instance.AddStatusEffect(monster.status, amplification);
        }
    }
}
