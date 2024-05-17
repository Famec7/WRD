using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            int xDirection = ownerTransform.localScale.x > 0 ? 1 : -1;
            List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(ownerTransform.position,
                new Vector2(3f, 1f), new Vector2(xDirection, 0));

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Monster monster))
                {
                    monster.HasAttacked(data.values[1]);
                    //Todo : 이펙트 추가
                }
            }

            return true;
        }

        return false;
    }
}