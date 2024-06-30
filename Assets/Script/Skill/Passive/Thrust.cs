using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            Vector3 dir = owner.Target.transform.position - owner.transform.position;
            List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(owner.transform.position,
                new Vector2(3f, 1f), dir);

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Monster monster))
                {
                    monster.HasAttacked(Data.GetValue(0));
                    //Todo : 이펙트 추가
                }
            }

            return true;
        }

        return false;
    }
}