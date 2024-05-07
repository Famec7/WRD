using System.Collections.Generic;
using UnityEngine;

public class CrossBow : RangedWeapon
{
    protected override void Attack()
    {
        base.Attack();
        float randomSeed = UnityEngine.Random.Range(0f, 100f);
        if (randomSeed < skillData.skillChance)
        {
            target.GetComponent<Monster>().HasAttacked(skillData.skillDamage);
        }
    }

    protected override void PassiveAura()
    {
        throw new System.NotImplementedException();
    }
}