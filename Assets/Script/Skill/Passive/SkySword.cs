using System.Collections;
using UnityEngine;

public class SkySword : PassiveSkillBase
{
    private WaitForSeconds _delay;

    protected override void Init()
    {
        base.Init();
        _delay = new WaitForSeconds(Data.GetValue(1));
    }

    public override bool Activate(GameObject target = null)
    {
        if (CheckTrigger())
        {
            var manager = ProjectileManager.Instance;

            var projectile = manager.CreateProjectile<SkySwordProjectile>("SkySword");

            if (projectile is null)
            {
                Debug.LogError("FallingSwordProjectile is not enough");
                return false;
            }

            projectile.SetPosition(weapon.owner.transform.position, weapon.owner.Target.transform.position);
            projectile.SetData(Data);

            return true;
        }

        return false;
    }
}