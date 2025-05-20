using UnityEngine;

public class Piercing : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (!CheckTrigger()) return false;
        
        if(target.TryGetComponent(out Monster monster))
        {
            OnHit(monster, Data.GetValue(0));
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