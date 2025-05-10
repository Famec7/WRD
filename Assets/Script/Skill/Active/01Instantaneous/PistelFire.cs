using System.Collections;
using UnityEngine;

public class PistelFire : InstantaneousSkill, IObserver
{
    [Header("장전하는 사운드")]
    [SerializeField]
    private AudioClip _reloadSound;
    
    private int _attackCount = 0;
    private float _originalAttackSpeed;

    protected override void Init()
    {
        base.Init();
        weapon.AddAction(OnNotify);
    }
    
    private void StatInit()
    {
        weapon.SetAttackDelay(_originalAttackSpeed);
    }

    public override void OnActiveEnter()
    {
        _attackCount = 0;
        
        _originalAttackSpeed = weapon.Data.AttackSpeed;
        weapon.SetAttackDelay(Data.GetValue(1));
    }

    public override bool OnActiveExecute()
    {
        if (_attackCount >= Data.GetValue(0))
        {
            /*weapon.owner.StartCoroutine(IE_DisableAttack());*/
            return true;
        }

        return false;
    }

    public override void OnActiveExit()
    {
        StatInit();
    }

    private IEnumerator IE_DisableAttack()
    {
        SoundManager.Instance.PlaySFX(_reloadSound);
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
        weapon.GetActiveSkill().CurrentCoolTime = isActive ? 0 : Data.GetValue(2);
    }
}