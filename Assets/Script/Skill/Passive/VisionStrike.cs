using System;
using System.Collections.Generic;
using UnityEngine;

public class VisionStrike : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (CheckTrigger())
        {
            /*List<Collider2D> targets = GetAttackTargets(target.transform.position,
                new Vector2(skillData.scopeRange, skillData.scopeRange));*/
            List<Collider2D> targets = GetAttackTargets(this.transform.position,
                new Vector2(3, 3));

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Status status))
                {
                    StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(tar.gameObject, 0f, 10));
                }
            }

            return true;
        }

        return false;
    }
}