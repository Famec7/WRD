﻿using UnityEngine;

public class Condemn : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if(!CheckTrigger() || target == null)
            return false;
        
        if (target.TryGetComponent(out Status status))
        {
            float stunDuration = Data.GetValue(0);
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, 100f, stunDuration));
            
            StatusEffectManager.Instance.AddStatusEffect(status, new Mark(status.gameObject, Data.GetValue(1)));
            
            var markStatus = StatusEffectManager.Instance.GetStatusEffect(status, typeof(Mark));
            if (markStatus != null)
            {
                if(status.TryGetComponent(out Monster monster))
                {
                    GuidedProjectile projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);
                    projectile.Target = target.gameObject;
                    
                    projectile.OnHit += () => OnHit(monster, Data.GetValue(2));
                }
            }
        }

        return true;
    }
    
    private void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);

        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
        particleEffect.SetPosition(monster.transform.position);
    }
}