using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : WeaponBase
{
    protected override void Attack()
    {
        //Todo: Add attack animation and effect
        Vector3 direction = owner.transform.localScale; // 펫이랑 플레이어에서 방향 알 수 있으면 수정 필요
        target = RangeDetectionUtility.GetAttackTargets(this.transform.position, direction, Data.attackRange, 360)[0].gameObject;
        
        if (target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.attackDamage);
        }
        else
        {
            target = null;
        }
    }
}