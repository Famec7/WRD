using UnityEngine;

public class HeavyBlow : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;

        Vector3 targetPosition = target.transform.position;
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, targetLayer);

        if (targets.Count == 0)
            return false;
        
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("HeavyBlowEffect");
        effect.SetPosition(targetPosition);
        effect.PlayEffect();

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));

                float slowDuration = Data.GetValue(1);
                StatusEffectManager.Instance.AddStatusEffect(monster.status,
                    new SlowDown(monster.gameObject, 100f, slowDuration));


                StatusEffect woundEffect = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

                if (woundEffect != null)
                {
                    monster.HasAttacked(Data.GetValue(2));

                    slowDuration = Data.GetValue(3);
                    float slowRate = Data.GetValue(4);

                    StatusEffectManager.Instance.AddStatusEffect(monster.status,
                        new SlowDown(monster.gameObject, slowRate, slowDuration));
                }
            }
        }

        return true;
    }
}