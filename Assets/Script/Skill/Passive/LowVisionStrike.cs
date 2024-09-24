using System;
using System.Collections.Generic;
using UnityEngine;

public class LowVisionStrike : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;
        
        Vector3 targetPosition = target.transform.position;
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, 360.0f, targetLayer);
        
        if(targets.Count == 0)
            return false;
        
        // 이펙트 재생
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("OrbHit");
        effect.SetPosition(target.transform.position);
        effect.SetScale(new Vector3(Data.Range, Data.Range, Data.Range));
        effect.PlayEffect();

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
                
                float slowDuration = Data.GetValue(1);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(tar.gameObject, 100f, slowDuration));
            }
        }

        return false;
    }
}