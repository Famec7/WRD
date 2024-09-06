using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    protected override void Attack()
    {
        base.Attack();
        
        anim.PlayAnimation();
        
        if (owner.Target.TryGetComponent(out Monster monster))
        {
#if WEAPON_DEBUG
            Debug.Log($"{Data.AttackDamage}의 데미지를 입힘");
#endif
            monster.HasAttacked(Data.AttackDamage);
        }
    }
}