using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LesserOrb : RangedWeapon
{
    private BoxCollider2D _collider2D;
    
    protected override void Init()
    {
        base.Init();
    }

    protected override void Attack()
    {
        base.Attack();
    }
}