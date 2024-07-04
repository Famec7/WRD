using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAuraSkillBase : SkillBase
{
    [HideInInspector] public PassiveAuraSkillData data;
    protected HashSet<string> tags = new HashSet<string> { "Monster", "Boss", "Mission" };

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        data = SkillDataManager.Instance.GetPassiveAuraSkillData(GetType().Name);
    }
}