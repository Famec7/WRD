public abstract class InstantaneousSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        SkillUIManager.Instance.ShowPopupPanel((int)ActiveStateType.Active);
        
        var currentSettingType = SettingManager.Instance.CurrentActiveSettingType;
        switch (currentSettingType)
        {
            case SettingManager.ActiveSettingType.Auto:
                ChangeState(new CastingState());
                break;
            case SettingManager.ActiveSettingType.SemiAuto:
                ChangeState(new ActiveState());
                break;
            case SettingManager.ActiveSettingType.Manual:
                ChangeState(new CastingState());
                break;
            default:
                break;
        }
    }

    /***************************State***************************/
}