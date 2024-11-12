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
    
    [Space] [Header("무기 종류에 맞는 사운드")]
    [SerializeField] private AudioClip _attackSound;

    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

            projectile.Target = owner.Target.gameObject;
            projectile.SetType(type);
            
            projectile.OnHit += () => OnHit(monster, Data.AttackDamage);
            
            SoundManager.Instance.PlaySFX(_attackSound);
        }
    }

    protected void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);
    }
}