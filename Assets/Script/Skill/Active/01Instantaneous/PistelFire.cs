using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    private float _attackOriginSpeed;
    private int _attackCount = 0;

    private void StatInit()
    {
        _attackCount = 0;
        _attackOriginSpeed = owner.Data.CurrentWeapon.Data.AttackSpeed;
    }
    
    public override void OnActiveEnter()
    {
        StatInit();
        owner.Data.CurrentWeapon.Data.AttackSpeed = Data.GetValue(1);
        
        if(owner.Data.CurrentWeapon is ISubject subject)
        {
            subject.AddObserver(this);
        }
    }

    public override void OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            ChangeState(new CoolTimeState());
        }
    }

    public override void OnActiveExit()
    {
        owner.Data.CurrentWeapon.Data.AttackSpeed = _attackOriginSpeed;
        owner.StartCoroutine(DisableAttack());
        
        if(owner.Data.CurrentWeapon is ISubject subject)
        {
            subject.RemoveObserver(this);
        }
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
    }
}