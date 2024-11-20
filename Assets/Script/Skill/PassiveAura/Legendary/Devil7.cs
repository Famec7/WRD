using System;
using System.Collections;
using UnityEngine;

public class Devil7 : PassiveAuraSkillBase
{
    private int _attackCount;

    private int AttackCount
    {
        get => _attackCount;
        set
        {
            _attackCount = value;
            
            if (_attackCount >= Data.GetValue(0))
            {
                OnActiveWeapon(false);
                StartCoroutine(IE_Reload());
            }
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

    private IEnumerator IE_Reload()
    {
        _attackCount = 0;
        
        float reloadTime = Data.GetValue(1);
        while (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
            yield return null;
        }
        
        OnActiveWeapon(true);
    }
    
    private void OnActiveWeapon(bool isActive)
    {
        weapon.enabled = isActive;

        weapon.activeSkill.enabled = !isActive;
    }
}