using System;
using System.Collections.Generic;
using UnityEngine;

public class LowVisionStrike : PassiveSkillBase
{
    private Vector2 _range;
    
    [SerializeField]
    private EffectBase _effect;

    protected override void Init()
    {
        base.Init();
        _range = new Vector2(Data.Range, Data.Range);
    }

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, Vector2.zero,
            _range, targetLayer);
        
        if(targets.Count == 0)
            return false;
        
        _effect.SetPosition(target.transform.position);
        _effect.SetScale(_range);
        _effect.PlayEffect();

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