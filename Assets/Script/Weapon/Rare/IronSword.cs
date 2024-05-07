using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSword : MeleeWeapon
{
    protected override void Attack()
    {
        if (passiveSkill.Activate())
            return;
        target.GetComponent<Monster>().HasAttacked(Data.attackDamage);
    }
}