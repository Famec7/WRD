using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAuraSkillBase : SkillBase
{
    private PassiveAuraSkillData _data;
    public PassiveAuraSkillData Data => _data;
    
    protected HashSet<string> tags = new HashSet<string> { "Monster", "Boss", "Mission" };

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        _data = SkillDataManager.Instance.GetPassiveAuraSkillData(GetType().Name);
    }
}