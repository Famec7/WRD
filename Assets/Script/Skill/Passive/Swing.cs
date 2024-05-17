using System.Collections.Generic;
using UnityEngine;

public class Swing : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (CheckTrigger())
        {
            // 왜 localscale로 방향을 판단하였는가...
            int xDirection = ownerTransform.localScale.x > 0 ? 1 : -1;
            List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(ownerTransform.position, new Vector2(1f, 3f), new Vector2(xDirection, 0));

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Status status) && tar.TryGetComponent(out Monster monster))
                {
                    StatusEffectManager.Instance.AddStatusEffect(status, new Wound(tar.gameObject));
                    monster.HasAttacked(data.values[1]);
                    // Todo : 이펙트 추가
                }
            }

            return true;
        }

        return false;
    }
}