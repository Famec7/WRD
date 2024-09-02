using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Swing : PassiveSkillBase
{
    private float _skillDamage;
    
    [SerializeField]
    private Vector2 _hitSize;
    
    /*******Effect*******/
    [SerializeField] private EffectBase _effect;
    
    protected override void Init()
    {
        base.Init();
        
        _skillDamage = Data.GetValue(0);
    }
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;
        if (dir == Vector3.zero)
            return false;
        
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _hitSize, dir, targetLayer);

        if(targets.Count == 0)
            return false;
        
        // 이펙트 재생
        _effect.SetPosition(weapon.owner.transform.position + dir);
        _effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.left, dir)));
        _effect.SetScale(_hitSize);
        _effect.PlayEffect();
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new Wound(tar.gameObject));
                monster.HasAttacked(_skillDamage);
                // Todo : 이펙트 추가
            }
        }

        return true;

    }
}