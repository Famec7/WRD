using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainVariation : PassiveSkillBase
{
    [SerializeField]
    private float _arrowJourneyTime = 1.0f;

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;

        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("ArrowRain");
        effect.SetPosition(target.transform.position);
        effect.PlayEffect();
        StartCoroutine(Damage(target.transform.position));
        return true;
    }


    IEnumerator Damage(Vector3 position)
    {
        yield return new WaitForSeconds(_arrowJourneyTime);
        var targets = RangeDetectionUtility.GetAttackTargets(position, Data.Range / 2f, default, targetLayer);

        if (targets.Count == 0)
            yield break;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
                Status status = monster.status;
                StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, 100f, Data.GetValue(1)));
            }
        }
    }
}
