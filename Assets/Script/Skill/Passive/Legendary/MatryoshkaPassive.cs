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
    
    private int _currentSlowRangeIndex = 0;
    
    private void SetSlowRange(int index)
    {
        if (index < 0 || index >= slowRanges.Count)
        {
            Debug.LogError("Index out of range for slowRanges.");
            return;
        }
        
        _currentSlowRangeIndex = index;
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
        if (!CheckTrigger() || target == null)
            return false;

        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, stunRange, default, targetLayer);

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
        
        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>("MatryoshkaPassive");
        particleEffect.SetColor(GetColor(_currentSlowRangeIndex));
        particleEffect.SetPosition(target.transform.position);

        return true;
    }

    private Color GetColor(int index)
    {
        switch (index)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.yellow;
            case 2:
                return Color.red;
        }
        
        return Color.white;
    }

    private void OnDisable()
    {
        _slowZone.StopEffect();
    }
}