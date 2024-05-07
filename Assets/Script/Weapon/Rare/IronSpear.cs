using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSpear : MeleeWeapon
{
    protected override void Attack()
    {
        if (target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.attackDamage);
            passiveSkill.Activate(target);
        }
    }
}