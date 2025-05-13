using System;

public class StaffOfHermes : PassiveAuraSkillBase
{
    
    CharacterController player;
    
    private float _attackSpeedMultiplier;
    private float _moveSpeedMultiplier;
    private float _originMoveSpeed;
    
    protected override void Init()
    {
        base.Init();
        
        _moveSpeedMultiplier = Data.GetValue(0) / 100.0f;
        _attackSpeedMultiplier = Data.GetValue(1) / 100.0f;
        player = CharacterManager.Instance.GetCharacter(CharacterManager.CharacterType.Player);
    }
    
    private void OnEnable()
    {
        ApplyBuff(_attackSpeedMultiplier);
        
        WeaponManager.Instance.OnWeaponEquipped += OnWeaponEquipped;
        
        _originMoveSpeed = player.Data.MoveSpeed;
        player.Data.MoveSpeed += player.Data.MoveSpeed * _moveSpeedMultiplier;
    }
    
    private void OnDisable()
    {
        RemoveBuff();
        
        WeaponManager.Instance.OnWeaponEquipped -= OnWeaponEquipped;
        
        player.Data.MoveSpeed -= _originMoveSpeed * _moveSpeedMultiplier;
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