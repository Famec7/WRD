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

    private void Start()
    {
        weapon.AddAction(OnAttack);
        _attackCount = 0;
    }

    private void OnAttack()
    {
        _attackCount++;
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
        weapon.activeSkill.CurrentCoolTime = isActive ? 0 : Data.GetValue(2);
    }
}