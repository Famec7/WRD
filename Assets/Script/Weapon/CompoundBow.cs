using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundBow : RangedWeapon
{
    public int markDamage = 50;

    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

            projectile.Target = owner.Target.gameObject;
            projectile.SetType(type);

            var mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));
            projectile.OnHit += () => OnHit(monster, Data.AttackDamage+markDamage);

            notifyAction?.Invoke();
        }
    }

    protected void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);

        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>(RangedWeapon.Type.Bow.ToString() + "Hit");
        particleEffect.SetPosition(monster.transform.position);
    }
}
