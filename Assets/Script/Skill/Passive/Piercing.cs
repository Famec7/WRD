using UnityEngine;

public class Piercing : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (!CheckTrigger()) return false;
        
        if(target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);

            projectile.Target = target.gameObject;
            projectile.Damage = Data.GetValue(0);
        }

        return false;
    }
}