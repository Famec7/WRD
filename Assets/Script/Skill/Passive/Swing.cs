using System.Collections.Generic;
using UnityEngine;

public class Swing : PassiveSkillBase
{
    private float _skillDamage;
    private Vector2 _hitSize;
    
    /*******Effect*******/
    [SerializeField] private SwingEffect _swingEffect;
    
    protected override void Init()
    {
        base.Init();
        
        _skillDamage = Data.GetValue(0);
        _hitSize = new Vector2(1f, 3f);
    }
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = owner.Target.transform.position - owner.transform.position;
        if (dir == Vector3.zero)
            return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(owner.transform.position, _hitSize, dir, targetLayer);

        if(targets.Count == 0)
            return false;
        
        _swingEffect.SetPosition(owner.transform.position + dir);
        _swingEffect.PlayEffect();
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                SetStatusEffect(monster, new Wound(tar.gameObject));
                monster.HasAttacked(_skillDamage);
                // Todo : 이펙트 추가
            }
        }

        return true;

    }
}