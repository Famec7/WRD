using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    private int _attackCount = 0;
    private bool _isEnter = false;

    private void StatInit()
    {
        _attackCount = 0;
        _isEnter = false;
        owner.Data.CurrentWeapon.AttackDelay = new WaitForSeconds(1 / owner.Data.CurrentWeapon.Data.AttackSpeed);
    }

    protected override INode.ENodeState OnActiveEnter()
    {
        if (_isEnter is false)
        {
            owner.Data.CurrentWeapon.AttackDelay = new WaitForSeconds(1 / Data.GetValue(1));

            if (owner.Data.CurrentWeapon is ISubject subject)
            {
                subject.AddObserver(this);
            }

            _isEnter = true;
        }

        return INode.ENodeState.Success;
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    protected override INode.ENodeState OnActiveExit()
    {
        StatInit();
        owner.StartCoroutine(DisableAttack());

        if (owner.Data.CurrentWeapon is ISubject subject)
        {
            subject.RemoveObserver(this);
        }

        IsActive = false;

        return INode.ENodeState.Success;
    }

    private IEnumerator DisableAttack()
    {
        owner.Data.CurrentWeapon.enabled = false;
        yield return new WaitForSeconds(Data.GetValue(2));
        owner.Data.CurrentWeapon.enabled = true;
    }

    public void OnNotify()
    {
        _attackCount++;
        Debug.Log(_attackCount);
    }
}