using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundBow : RangedWeapon
{
    public bool IsMarked { get; set; }
    
    private float _originalDamage;

    protected override void Init()
    {
        base.Init();
        _originalDamage = Data.AttackDamage;
    }

    protected override void Attack()
    {
        if (IsMarked)
        {
            Data.AttackDamage = _originalDamage + passiveAuraSkill.Data.GetValue(0);
        }
        else
        {
            Data.AttackDamage = _originalDamage;
        }
        
        base.Attack();
    }
}
