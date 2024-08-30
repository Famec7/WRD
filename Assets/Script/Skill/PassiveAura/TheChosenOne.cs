using UnityEngine;

public class TheChosenOne : PassiveAuraSkillBase
{
    private void OnEnable()
    {
        ApplyBuff(data.GetValue(0), data.GetValue(1));
    }
    
    private void OnDisable()
    {
        ApplyBuff(1 / data.GetValue(0), 1 / data.GetValue(1));
    }
    
    private void ApplyBuff(float damageMultiplier, float attackSpeedMultiplier)
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            // 플레이어와 펫의 공격력 변경
            CharacterManager.Instance.GetCharacter(i).Data.CurrentWeapon.Data.AttackDamage *= damageMultiplier;

            // 플레이어와 펫의 공격속도 변경
            var attackSpeed = CharacterManager.Instance.GetCharacter(i).Data.CurrentWeapon.Data.AttackSpeed;
            CharacterManager.Instance.GetCharacter(i).Data.CurrentWeapon.AttackDelay = new WaitForSeconds(1 / attackSpeed * attackSpeedMultiplier);
        }
    }
}