using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedWeapon : WeaponBase
{
    public enum Type
    {
        Bow,
        Gun,
        Orb,
    }
    
    [SerializeField]
    private Type type;
    
    protected override void Attack()
    {
        base.Attack();
        
        anim.PlayAnimation();

        if (owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

            projectile.Target = owner.Target.gameObject;
            projectile.Damage = Data.AttackDamage;
            
            projectile.SetType(type);

            notifyAction?.Invoke();
        }
    }
}