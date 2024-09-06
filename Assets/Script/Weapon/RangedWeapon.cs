using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangedWeapon : WeaponBase
{
    protected override void Attack()
    {
        base.Attack();
        
        anim.PlayAnimation();

        if (owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, owner.transform.position);

            projectile.Target = owner.Target.gameObject;
            projectile.Damage = Data.AttackDamage;

            notifyAction?.Invoke();
        }
    }
}