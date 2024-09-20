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
        if (!CheckTrigger()) return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, Vector2.zero,
            _range, targetLayer);
        
        if(targets.Count == 0)
            return false;
        
        // 이펙트 재생
        HitEffect effect = EffectManager.Instance.CreateEffect<HitEffect>("OrbHit");
        effect.SetPosition(target.transform.position);
        effect.SetScale(_range);
        effect.PlayEffect();

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