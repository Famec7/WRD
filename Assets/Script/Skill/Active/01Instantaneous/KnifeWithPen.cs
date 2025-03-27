using UnityEngine;

public class KnifeWithPen : InstantaneousSkill
{
    [SerializeField]
    private bool _isMelee;
    
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
    
    private void ChangeWeapon(bool isMelee)
    {
        const int characterType = (int) CharacterManager.CharacterType.Player;
        WeaponManager.Instance.RemoveWeapon(characterType);
        if (isMelee)
        {
            WeaponManager.Instance.AddWeapon(characterType, _meleeIndex);
        }
        else
        {
            WeaponManager.Instance.AddWeapon(characterType, _rangeIndex);
        }
        
        WeaponBase weapon = WeaponManager.Instance.GetEquippedWeapon(characterType);
        weapon.GetActiveSkill(1).CurrentCoolTime = 0.0f;
    }
}