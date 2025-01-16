using System.Collections.Generic;

public abstract class InstantaneousSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        var currentSettingType = SettingManager.Instance.CurrentActiveSettingType;
        
        switch (currentSettingType)
        {
            case SettingManager.ActiveSettingType.SemiAuto:
                _commandInvoker.SetCommand(new ActiveSkillCommand(this));
                break;
            case SettingManager.ActiveSettingType.Manual:
                _commandInvoker.SetCommand(new IndicatorCommand(this));
                break;
            case SettingManager.ActiveSettingType.Auto:
            default:
                break;
        }
    }
}