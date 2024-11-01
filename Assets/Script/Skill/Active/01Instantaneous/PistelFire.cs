using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    private int _attackCount = 0;

    private void StatInit()
    {
        _attackCount = 0;
        weapon.SetAttackDelay(weapon.owner.Data.CurrentWeapon.Data.AttackSpeed);
    }

    protected override void OnActiveEnter()
    {
        weapon.SetAttackDelay(Data.GetValue(1));
        weapon.AddAction(OnNotify);
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            weapon.owner.StartCoroutine(IE_DisableAttack());
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    protected override void OnActiveExit()
    {
        StatInit();
    }

    private IEnumerator IE_DisableAttack()
    {
        OnActiveWeapon(false);
        yield return new WaitForSeconds(Data.GetValue(2));
        OnActiveWeapon(true);
    }

    public void OnNotify()
    {
        _attackCount++;
    }
    
    private void OnActiveWeapon(bool isActive)
    {
        weapon.enabled = isActive;
        weapon.activeSkill.CurrentCoolTime = isActive ? 0 : Data.GetValue(2);
    }
}