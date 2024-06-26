using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    protected override void Attack()
    {
        if (target is null)
            return;
        
        Debug.Log("MeleeWeapon Attack");
        if (passiveSkill.Activate())
            return;
        
        //Todo: Add attack animation and effect
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