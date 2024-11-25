using System;
using System.Collections;
using UnityEngine;

public class Devil7 : PassiveAuraSkillBase
{
    private int _attackCount;

    public int AttackCount
    {
        get => _attackCount;
        set
        {
            _attackCount = value;
        }
    }

    protected override void Init()
    {
        base.Init();
        
        weapon.AddAction(OnAttack);
        _attackCount = 0;
    }

    private void Start()
    {
        OnActiveWeapon(true);
    }

    private void OnAttack()
    {
        AttackCount++;
    }
    
    public void OnActiveWeapon(bool isActive)
    {
        weapon.enabled = isActive;
    }
}