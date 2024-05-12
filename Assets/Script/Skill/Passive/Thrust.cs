using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            int xDirection = ownerTransform.localScale.x > 0 ? 1 : -1;
            List<Collider2D> targets = GetAttackTargets(ownerTransform.position + new Vector3(xDirection, 0),
                new Vector2(3, 1));

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