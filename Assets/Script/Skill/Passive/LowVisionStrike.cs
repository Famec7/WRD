using System;
using System.Collections.Generic;
using UnityEngine;

public class LowVisionStrike : PassiveSkillBase
{
    private Vector2 _range = new Vector2(1f, 1f);
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(target.transform.position,
            _range, targetLayer);

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Status status))
            {
                StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(tar.gameObject, 100f, 0.3f));
            }
        }

        return false;
    }
}