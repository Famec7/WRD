using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TripleKunai : PassiveSkillBase
{
    private readonly List<Monster> _hitMonsterList = new List<Monster>();
    [SerializeField]
    private AudioClip _skillSound;
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null) return false;
        if (target.GetComponent<Status>().HP <= 0) return false;
        target.GetComponent<SpriteRenderer>().color = Color.red;
        _hitMonsterList.Clear();
        _hitMonsterList.Add(weapon.owner.Target.GetComponent<Monster>());
 
        for (int i = 0; i < 3; i++)
        {
            ColliderProjectile projectile = ProjectileManager.Instance
                .CreateProjectile<ColliderProjectile>(default, this.transform.position);

            float angle = Vector3.SignedAngle(
                transform.up,
                weapon.owner.Target.transform.position - weapon.owner.transform.position,
                Vector3.forward
            );

            float moveAngle = angle + (-15f + 15f * i);
            float finalAngle = moveAngle + 90f;

            projectile.transform.rotation = Quaternion.Euler(0, 0, finalAngle);
            projectile.Init(weapon.Data.AttackDamage, finalAngle);
            projectile.SetType(RangedWeapon.Type.Bow);
            projectile.OnHit += () => OnHit(projectile);
        }
        SoundManager.Instance.PlaySFX(_skillSound);

        return true;
    }

    public virtual void OnHit(ColliderProjectile projectile)
    {
        projectile._damage  = weapon.Data.AttackDamage * (1 -Data.GetValue(0) * 0.01f);
    }
}
