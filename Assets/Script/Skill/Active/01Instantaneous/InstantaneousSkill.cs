using System.Collections.Generic;

public abstract class InstantaneousSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        var currentSettingType = SettingManager.Instance.CurrentActiveSettingType;
        
        switch (currentSettingType)
        {
            case SettingManager.ActiveSettingType.Auto:
                IsIndicatorState = true;
                break;
            case SettingManager.ActiveSettingType.SemiAuto:
                IsCoolTime = false;
                IsActive = true;
                break;
            case SettingManager.ActiveSettingType.Manual:
                IsCoolTime = false;
                IsIndicatorState = true;
                break;
            default:
                break;
        }
    }

    /***************************Behaviour Tree***************************/
    protected override INode SettingBT()
    {
        return new SelectorNode(
            new List<INode>()
            {
                new SequenceNode(
                    CoolTimeNodes()
                ),
                new SequenceNode(
                    IndicatorNodes()
                ),
                new SequenceNode(
                    ActiveNodes()
                )
            }
        );
    }
}