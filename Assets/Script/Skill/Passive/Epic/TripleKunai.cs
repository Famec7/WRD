using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TripleKunai : PassiveSkillBase
{
    private readonly List<Monster> _hitMonsterList = new List<Monster>();
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;

        _hitMonsterList.Clear();
        _hitMonsterList.Add(weapon.owner.Target.GetComponent<Monster>());

        for (int i = 0; i < 3; i++)
        {
            ColliderProjectile projectile = ProjectileManager.Instance.CreateProjectile<ColliderProjectile>(default, this.transform.position);
            float moveAngle = -15f + 15f*i;
            projectile.gameObject.transform.rotation = Quaternion.Euler(0,0,moveAngle);
            projectile.Init(weapon.Data.AttackDamage, moveAngle);
            projectile.SetType(RangedWeapon.Type.Bow);
            projectile.OnHit += () => OnHit(projectile);
        }

        return true;
    }

    public void OnHit(ColliderProjectile projectile)
    {
        projectile._damage  = weapon.Data.AttackDamage * (1 -Data.GetValue(0) * 0.01f);
    }
}
