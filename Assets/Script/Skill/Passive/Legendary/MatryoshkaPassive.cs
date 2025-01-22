using System;
using UnityEngine;

public class MatryoshkaPassive : PassiveSkillBase
{
    [SerializeField] private float stunRange = 3.0f;

    private float _slowRange = 0.0f;
    
    private CircleCollider2D _collider;
    
    private void SetSlowRange(float value)
    {
        _slowRange = value;
        _collider.radius = _slowRange / 2;   
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

        foreach (var t in targets)
        {
            if (t.TryGetComponent(out Status status))
            {
                StatusEffect stun = new SlowDown(t.gameObject, 100f, Data.GetValue(1));
                StatusEffectManager.Instance.AddStatusEffect(status, stun);
            }
        }

        return true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffect slow = new SlowDown(other.gameObject, Data.GetValue(0));
            StatusEffectManager.Instance.AddStatusEffect(status, slow);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
        }
    }
}