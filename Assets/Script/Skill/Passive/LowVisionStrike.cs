using System;
using System.Collections.Generic;
using UnityEngine;

public class LowVisionStrike : PassiveSkillBase
{
    private Vector3 _range;

    protected override void Init()
    {
        base.Init();
        _range = new Vector3(Data.Range, Data.Range, Data.Range);
    }

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;
        
        Vector3 targetPosition = target.transform.position;
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Vector2.zero, _range, targetLayer);
        
        if(targets.Count == 0)
            return false;
        
        // 이펙트 재생
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("OrbHit");
        effect.SetPosition(target.transform.position);
        effect.SetScale(_range);
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