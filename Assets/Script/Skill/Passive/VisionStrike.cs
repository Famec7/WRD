﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class VisionStrike : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (target is null)
            return false;
        
        if (CheckTrigger())
        {
            List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(target.transform.position,
                new Vector2(1f, 1f));

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Status status))
                {
                    StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(tar.gameObject, 0f, 0.3f));
                }
            }

            return true;
        }

        return false;
    }
}