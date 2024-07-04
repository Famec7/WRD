using System.Collections.Generic;
using UnityEngine;

public class Thrust : PassiveSkillBase
{
    private Vector2 _range = new Vector2(3f, 1f);
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        
        Vector3 dir = target.transform.position - owner.transform.position;
        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(owner.transform.position,
            _range, dir, targetLayer);

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
}