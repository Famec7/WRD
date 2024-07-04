using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LowOrb : RangedWeapon
{
    protected override void Attack()
    {
        base.Attack();
        
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.attackDamage);
        }
    }
}