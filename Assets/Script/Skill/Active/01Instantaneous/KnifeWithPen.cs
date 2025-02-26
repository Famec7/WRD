using UnityEngine;

public class KnifeWithPen : InstantaneousSkill
{
    private bool _isMelee = true;
    
    [SerializeField]
    private int _meleeIndex;
    [SerializeField]
    private int _rangeIndex;
    
    public override void OnActiveEnter()
    {
        ChangeWeapon(!_isMelee);
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
    
    private void ChangeWeapon(bool isMelee = true)
    {
        _isMelee = isMelee;
        
        const int characterType = (int) CharacterManager.CharacterType.Player;
        WeaponManager.Instance.RemoveWeapon(characterType);
        if (_isMelee)
        {
            WeaponManager.Instance.AddWeapon(characterType, _meleeIndex);
        }
        else
        {
            WeaponManager.Instance.AddWeapon(characterType, _rangeIndex);
        }
        
        WeaponBase weapon = WeaponManager.Instance.GetEquippedWeapon(characterType);
        weapon.GetActiveSkill(0).CurrentCoolTime = 0.0f;
    }
}