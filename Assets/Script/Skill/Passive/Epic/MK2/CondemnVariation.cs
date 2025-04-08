using UnityEngine;

public class CondemnVariation : Condemn
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;

        if (target.TryGetComponent(out Monster monster))
        {
            GuidedProjectile projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);
            projectile.Target = target.gameObject;
                    
            projectile.OnHit += () => OnHit(monster, Data.GetValue(0));
        }
        
        return true;
    }

    protected override void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);
            
        Stun stun = new Stun(monster.gameObject, Data.GetValue(1));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
        
        var markStatus = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));
        if (markStatus != null)
        {
            monster.HasAttackedPercent(Data.GetValue(2));
        }
    }
}