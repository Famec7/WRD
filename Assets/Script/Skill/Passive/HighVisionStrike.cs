using UnityEngine;

public class HighVisionStrike : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;
        
        Vector3 targetPosition = target.transform.position;
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, LayerMaskManager.Instance.MonsterLayerMask);
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(monster.gameObject, 100f, Data.GetValue(1)));
                
                ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("HighOrbHit");
                effect.SetPosition(target.transform.position);
                
                Vector3 range = new Vector3(Data.Range, Data.Range, Data.Range);
                effect.SetScale(range);
                effect.PlayEffect();
            }
        }
        
        return true;
    }
}