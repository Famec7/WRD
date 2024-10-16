﻿using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    private int _attackCount = 0;

    private void StatInit()
    {
        _attackCount = 0;
        weapon.owner.Data.CurrentWeapon.SetAttackDelay(weapon.owner.Data.CurrentWeapon.Data.AttackSpeed);
    }

    protected override void OnActiveEnter()
    {
        weapon.owner.Data.CurrentWeapon.SetAttackDelay(Data.GetValue(1));
        weapon.owner.Data.CurrentWeapon.AddAction(OnNotify);
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            IsActive = false;
            weapon.owner.StartCoroutine(IE_DisableAttack());
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    protected override void OnActiveExit()
    {
        StatInit();
        weapon.owner.Data.CurrentWeapon.RemoveAction(OnNotify);
    }

    private IEnumerator IE_DisableAttack()
    {
        weapon.owner.Data.CurrentWeapon.enabled = false;
        yield return new WaitForSeconds(Data.GetValue(2));
        weapon.owner.Data.CurrentWeapon.enabled = true;
    }

    public void OnNotify()
    {
        _attackCount++;
    }
}