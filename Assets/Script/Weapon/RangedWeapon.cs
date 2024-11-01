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
        HighOrb,
    }

    [SerializeField] protected Type type;

    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

            projectile.Target = owner.Target.gameObject;
            projectile.SetType(type);
            
            projectile.OnHit += () => OnHit(monster, Data.AttackDamage);
        }
    }

    protected void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);
    }
}