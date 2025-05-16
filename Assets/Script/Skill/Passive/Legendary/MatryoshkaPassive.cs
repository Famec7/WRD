using System;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaPassive : PassiveSkillBase
{
    [Header("기절 범위")]
    [SerializeField]
    private float stunRange = 1.0f;
    
    [Header("이속 감소 범위")]
    [SerializeField]
    private List<float> slowRanges;
    
    [SerializeField] private SlowZone _slowZone;
    
    private void SetSlowRange(int index)
    {
        if (index < 0 || index >= slowRanges.Count)
        {
            Debug.LogError("Index out of range for slowRanges.");
            return;
        }
        
        float value = slowRanges[index];
        _slowZone.SetData(0, value, Data.GetValue(0));
        
        _slowZone.PlayEffect();
    }
    
    private void Start()
    {
        if (weapon.GetActiveSkill() is MatryoshkaActive activeSkill)
        {
            activeSkill.SetSlowRange = SetSlowRange;
        }
    }

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger())
            return false;

        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, stunRange, default, targetLayer);

        if (targets.Count == 0)
            return false;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Status status))
            {
                StatusEffect stun = new Stun(tar.gameObject, Data.GetValue(2));
                StatusEffectManager.Instance.AddStatusEffect(status, stun);
            }

            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(1));
            }
        }

        return true;
    }

    private void OnDisable()
    {
        _slowZone.StopEffect();
    }
}