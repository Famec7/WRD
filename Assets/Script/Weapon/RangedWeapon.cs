using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : WeaponBase
{
    protected override void Attack()
    {
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

    protected override void PassiveAura()
    {
        ;
    }
}