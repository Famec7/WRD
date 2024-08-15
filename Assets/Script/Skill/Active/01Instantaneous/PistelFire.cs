using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    private int _attackCount = 0;

    private void StatInit()
    {
        _attackCount = 0;
        owner.Data.CurrentWeapon.AttackDelay = new WaitForSeconds(1 / owner.Data.CurrentWeapon.Data.AttackSpeed);
    }

    protected override void OnActiveEnter()
    {
        owner.Data.CurrentWeapon.AttackDelay = new WaitForSeconds(1 / Data.GetValue(1));
        owner.Data.CurrentWeapon.AddAction(OnNotify);
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            IsActive = false;
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    protected override void OnActiveExit()
    {
        StatInit();
        owner.StartCoroutine(IE_DisableAttack());
        
        owner.Data.CurrentWeapon.RemoveAction(OnNotify);
    }

    private IEnumerator IE_DisableAttack()
    {
        owner.Data.CurrentWeapon.enabled = false;
        yield return new WaitForSeconds(Data.GetValue(2));
        owner.Data.CurrentWeapon.enabled = true;
    }

    public void OnNotify()
    {
        _attackCount++;
    }
}