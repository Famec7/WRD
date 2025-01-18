using System.Collections.Generic;

public abstract class InstantaneousSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        var currentSettingType = SettingManager.Instance.CurrentActiveSettingType;
        
        switch (currentSettingType)
        {
            case SettingManager.ActiveSettingType.SemiAuto:
                commandInvoker.AddCommand(new ActiveSkillCommand(this));
                break;
            case SettingManager.ActiveSettingType.Manual:
                commandInvoker.AddCommand(new IndicatorCommand(this));
                break;
            case SettingManager.ActiveSettingType.Auto:
            default:
                break;
        }
    }
}