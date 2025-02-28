using System;
using UnityEngine;

public class MatryoshkaPassive : PassiveSkillBase
{
    [SerializeField] private float stunRange = 3.0f;
    [SerializeField] private SlowZone _slowZone;

    private float _slowRange = 0.0f;
    
    private CircleCollider2D _collider;
    
    private void SetSlowRange(float value)
    {
        _slowRange = value;
        _slowZone.SetData(0, value, Data.GetValue(1));
        _slowZone.transform.SetParent(weapon.owner.transform);
        
        _slowZone.PlayEffect();
    }
    
    protected override void Init()
    {
        base.Init();
        
        targetLayer = LayerMaskProvider.MonsterLayerMask;
        _collider = GetComponent<CircleCollider2D>();
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
                monster.HasAttacked(Data.GetValue(0));
            }
        }

        return true;
    }

    private void OnDisable()
    {
        _slowZone.StopEffect();
    }
}