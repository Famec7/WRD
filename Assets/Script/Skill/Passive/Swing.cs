using System.Collections.Generic;
using UnityEngine;

public class Swing : PassiveSkillBase
{
    private int _skillDamage;
    private Vector2 _hitSize;
    
    protected override void Init()
    {
        base.Init();
        
        _skillDamage = data.values[1];
        _hitSize = new Vector2(1f, 3f);
    }
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = PlayerManager.instance.DirToTarget;
        if (dir == Vector3.zero)
            return false;
            
        Debug.Log("Swing Activate");
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(ownerTransform.position, _hitSize, dir);

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Status status) && tar.TryGetComponent(out Monster monster))
            {
                StatusEffectManager.Instance.AddStatusEffect(status, new Wound(tar.gameObject));
                monster.HasAttacked(_skillDamage);
                // Todo : 이펙트 추가
            }
        }

        return true;

    }
}