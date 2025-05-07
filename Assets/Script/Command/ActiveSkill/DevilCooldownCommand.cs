using System.Collections;
using UnityEngine;

public class DevilCooldownCommand : CooldownCommand
{
    private float _checkTime;

    public DevilCooldownCommand(ActiveSkillBase skill) : base(skill)
    {
        // Devil7의 특수 데이터 가져오기
        if (skill.weapon.GetPassiveAuraSkill() is Devil7 devil7)
        {
            _checkTime = devil7.Data.GetValue(1);
            devil7.AttackCount = 0;
        }
    }

    public override bool Execute()
    {
        if (skill.weapon.GetPassiveAuraSkill() is Devil7 devil7)
        {
            // 쿨타임 계산
            skill.CurrentCoolTime = devil7.Data.GetValue(0) - devil7.AttackCount;
        }

        return skill.CurrentCoolTime <= 0;
    }

    public override void OnComplete()
    {
        base.OnComplete();

        skill.CurrentCoolTime = 0;

        var setting = SettingManager.Instance.CurrentActiveSettingType;

        if (setting != SettingManager.ActiveSettingType.Auto)
        {
            skill.StartCoroutine(CheckSkillInactivity());
        }
    }

    private IEnumerator CheckSkillInactivity()
    {
        yield return new WaitForSeconds(_checkTime);
        
        // 스킬을 사용하지 않으면 CancelSkill 실행
        if (skill.IsActive is false)
        {
            skill.CurrentCoolTime = skill.Data.CoolTime;
            skill.CancelSkill();
        }
    }
}