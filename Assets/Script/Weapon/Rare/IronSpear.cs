using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronSpear : MeleeWeapon
{
    protected override void Attack()
    {
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.attackDamage);
            passiveSkill.Activate(owner.Target);
        }
    }
}