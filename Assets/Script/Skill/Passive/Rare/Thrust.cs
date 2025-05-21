using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = target.transform.position - weapon.owner.transform.position;
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, Data.Range, 360.0f, targetLayer);

        if(targets.Count == 0)
            return false;

        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("SpearEffect");

        effect.SetPosition(weapon.owner.transform.position);
        effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir)));
        effect.PlayEffect();
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));
            }
        }

        return true;

    }
}