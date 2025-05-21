using System;

public class StaffOfHermes : PassiveAuraSkillBase
{
    private float _attackSpeedMultiplier;
    private float _originMoveSpeed;
    
    protected override void Init()
    {
        base.Init();
        
        _attackSpeedMultiplier = Data.GetValue(0) / 100.0f;
    }
    
    private void OnEnable()
    {
        ApplyBuff(_attackSpeedMultiplier);
        
        WeaponManager.Instance.OnWeaponEquipped += OnWeaponEquipped;
    }
    
    private void OnDisable()
    {
        RemoveBuff();
        
        WeaponManager.Instance.OnWeaponEquipped -= OnWeaponEquipped;
    }
    
    private void ApplyBuff(float attackSpeedMultiplier)
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            
            ApplyBuffToCharacter(character);
        }
    }
    
    private void ApplyBuffToCharacter(CharacterController character)
    {
        if (character.Data.CurrentWeapon is null)
        {
            return;
        }
        
        float attackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
        character.Data.CurrentWeapon.SetAttackDelay(attackSpeed * (1 + _attackSpeedMultiplier));
    }
    
    private void RemoveBuff()
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            
            RemoveBuffFromCharacter(character);
        }
    }
    
    private void RemoveBuffFromCharacter(CharacterController character)
    {
        if (character.Data.CurrentWeapon is null)
        {
            return;
        }
        
        float attackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
        character.Data.CurrentWeapon.SetAttackDelay(attackSpeed / (1 + _attackSpeedMultiplier));
    }
    
    private void OnWeaponEquipped(CharacterController character)
    {
        ApplyBuffToCharacter(character);
    }
    
    private void OnWeaponDetached(CharacterController character)
    {
        RemoveBuffFromCharacter(character);
    }
}