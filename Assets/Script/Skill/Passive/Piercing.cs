using UnityEngine;

public class Piercing : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (!CheckTrigger()) return false;
        
        if(target.TryGetComponent(out Monster monster))
        {
            var projectile = ProjectileManager.Instance.CreateProjectile<GuidedProjectile>(default, this.transform.position);
            
            projectile.OnHit += () => OnHit(monster, Data.GetValue(0));

            projectile.Target = target.gameObject;
        }

        return false;
    }
    
    private void OnHit(Monster monster, float damage)
    {
        monster.HasAttacked(damage);

        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>("PiercingEffect");
        particleEffect.SetPosition(monster.transform.position);
    }
}