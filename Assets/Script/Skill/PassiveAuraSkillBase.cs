using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAuraSkillBase : SkillBase
{
    public PassiveAuraSkillData Data { get; private set; }

    protected HashSet<string> tags = new HashSet<string> { "Monster", "Boss", "Mission" };

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Data = SkillManager.Instance.GetPassiveAuraSkillData(GetType().Name);
    }
}