using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    protected override void Attack()
    {
        base.Attack();
        
        //Todo: Add attack animation and effect
        if (owner.Target.TryGetComponent(out Monster monster))
        {
#if WEAPON_DEBUG
            Debug.Log($"{Data.attackDamage}의 데미지를 입힘");
#endif
            monster.HasAttacked(Data.attackDamage);
        }
    }
}