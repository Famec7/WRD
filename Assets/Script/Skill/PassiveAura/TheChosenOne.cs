using UnityEngine;

public class TheChosenOne : PassiveAuraSkillBase
{
    private float _damageMultiplier;
    private float _attackSpeedMultiplier;
    
    protected override void Init()
    {
        base.Init();
        
        _damageMultiplier = Data.GetValue(0);
        _attackSpeedMultiplier = Data.GetValue(1);
    }
    
    private void OnEnable()
    {
        if(Data == null)
        {
            return;
        }
        
        ApplyBuff(_damageMultiplier, _attackSpeedMultiplier);
    }

    private void OnDisable()
    {
        if(Data == null)
        {
            return;
        }
        
        ApplyBuff(1 / _damageMultiplier, 1 / _attackSpeedMultiplier);
    }
    
    private void ApplyBuff(float damageMultiplier, float attackSpeedMultiplier)
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            var character = CharacterManager.Instance.GetCharacter(i);
            
            if (character.Data.CurrentWeapon == null)
            {
                continue;
            }
            
            // 플레이어와 펫의 공격력 변경
            character.Data.CurrentWeapon.Data.AttackDamage *= damageMultiplier;

            // 플레이어와 펫의 공격속도 변경
            var attackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
            character.Data.CurrentWeapon.SetAttackDelay(attackSpeed * attackSpeedMultiplier);
        }
    }
}