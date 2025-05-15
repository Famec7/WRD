using UnityEngine;

public class TheChosenOne : PassiveAuraSkillBase
{
    private float _damageMultiplier;
    private float _attackSpeedMultiplier;
    
    protected override void Init()
    {
        base.Init();
        
        _damageMultiplier = Data.GetValue(0) / 100.0f;
        _attackSpeedMultiplier = Data.GetValue(1) / 100.0f;
    }
    
    private void OnEnable()
    {
        if(Data == null)
        {
            return;
        }
        
        ApplyBuff(_damageMultiplier, _attackSpeedMultiplier);
        
        WeaponManager.Instance.OnWeaponEquipped += OnWeaponEquipped;
        WeaponManager.Instance.OnWeaponDetached += OnWeaponDetached;
    }

    private void OnDisable()
    {
        if(Data == null)
        {
            return;
        }
        
        RemoveBuff();
        
        WeaponManager.Instance.OnWeaponEquipped -= OnWeaponEquipped;
        WeaponManager.Instance.OnWeaponDetached -= OnWeaponDetached;
    }
    
    private void ApplyBuff(float damageMultiplier, float attackSpeedMultiplier)
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            
            ApplyBuffToCharacter(character);
        }
    }
    
    private void ApplyBuffToCharacter(CharacterController character)
    {
        if (character.Data.CurrentWeapon == null)
        {
            return;
        }
        
        // 플레이어와 펫의 공격력 변경
        character.Data.CurrentWeapon.Data.AttackDamage *= (1 + _damageMultiplier);

        // 플레이어와 펫의 공격속도 변경
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
        if (character.Data.CurrentWeapon == null)
        {
            return;
        }
        
        // 플레이어와 펫의 공격력 변경
        character.Data.CurrentWeapon.Data.AttackDamage /= (1 + _damageMultiplier);

        // 플레이어와 펫의 공격속도 변경
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