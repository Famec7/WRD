using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Lancer : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null) return false;

        StatusEffect woundEffect = StatusEffectManager.Instance.GetStatusEffect(weapon.owner.Target.GetComponent<Monster>().status, typeof(Wound));

        if (woundEffect != null)
            return false;

        weapon.GetActiveSkill().CurrentCoolTime -= Data.GetValue(0);

        return true;
    }
}
