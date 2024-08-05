using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RangedWeapon : WeaponBase
{

    protected override void Attack()
    {
        base.Attack();
        //Todo: Add attack animation and effect

        if (owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.AttackDamage);
            notifyAction?.Invoke();
        }
    }
}