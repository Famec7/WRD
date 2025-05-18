using UnityEngine;

public class HeartQueenEffect : CardEffectBase
{
    private float _time;
    
    public HeartQueenEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData(12);
        _time = Data.GetValue(0);
    }

    public override void OnEnter()
    {
        // 아군 무기들의 공격속도가 10초 동안 100% 빨라진다. 또한 확률로 발동하는 스킬들의 확률이 20%p 상승한다.

        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            WeaponBase weapon = character.Data.CurrentWeapon;
            
            if (weapon == null)
            {
                continue;
            }
            
            ApplyBuffToCharacter(character);
        }

        WeaponManager.Instance.OnWeaponEquipped += ApplyBuffToCharacter;
        WeaponManager.Instance.OnWeaponDetached += RemoveBuffFromCharacter;
    }

    public override bool OnUpdate()
    {
        if (_time <= 0)
        {
            return true;
        }
        
        _time -= Time.deltaTime;
        return false;
    }

    public override void OnExit()
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            WeaponBase weapon = character.Data.CurrentWeapon;
            
            if (weapon == null)
            {
                continue;
            }
            
            RemoveBuffFromCharacter(character);
        }
        
        WeaponManager.Instance.OnWeaponEquipped -= ApplyBuffToCharacter;
        WeaponManager.Instance.OnWeaponDetached -= RemoveBuffFromCharacter;
    }
    
    private void ApplyBuffToCharacter(CharacterController character)
    {
        WeaponBase weapon = character.Data.CurrentWeapon;
        
        float originalAttackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
        originalAttackSpeed += Data.GetValue(1) / 100.0f;
        weapon.SetAttackDelay(originalAttackSpeed);

        weapon.GetPassiveSkill().Data.Chance += (int)Data.GetValue(2);
    }
    
    private void RemoveBuffFromCharacter(CharacterController character)
    {
        WeaponBase weapon = character.Data.CurrentWeapon;
        
        float originalAttackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
        originalAttackSpeed -= Data.GetValue(1) / 100.0f;
        weapon.SetAttackDelay(originalAttackSpeed);

        weapon.GetPassiveSkill().Data.Chance -= (int)Data.GetValue(2);
    }
}