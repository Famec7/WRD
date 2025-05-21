using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainVariation : PassiveSkillBase
{
    [SerializeField] private AudioClip sfx;

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;

        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("ArrowRain");
        effect.SetPosition(target.transform.position);
        effect.PlayEffect();
        SoundManager.Instance.PlaySFX(sfx);
        Damage(target.transform.position);
        return true;
    }


    private void Damage(Vector3 position)
    {
        var targets = RangeDetectionUtility.GetAttackTargets(position, Data.Range / 2f, default, targetLayer);

        if (targets.Count == 0)
            return;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
                Status status = monster.status;
                StatusEffectManager.Instance.AddStatusEffect(status, new Stun(status.gameObject, Data.GetValue(1)));
            }
        }
    }
}
