using UnityEngine;

public class Stone : RangedWeapon
{
    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            GuidedProjectile projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>("Stone", this.transform.position);
            
            projectile.Target = monster.gameObject;
            projectile.SetType(type);
            projectile.OnHit += () => OnHit(monster, Data.AttackDamage);
            
            PlayAttackSound();
        }
    }
}