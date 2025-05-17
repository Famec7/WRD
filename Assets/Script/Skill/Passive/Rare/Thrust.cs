using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    [SerializeField]
    private Vector2 _range = new Vector2(3.0f, 1.0f);
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = target.transform.position - weapon.owner.transform.position;
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _range, dir, targetLayer);

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