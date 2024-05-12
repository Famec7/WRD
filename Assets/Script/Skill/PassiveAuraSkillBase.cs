using UnityEngine;

public abstract class PassiveAuraSkillBase : SkillBase
{
    [HideInInspector] public PassiveSkillData data;
    
    private void Awake()
    {
        Init();
    }
    
    protected override void Init()
    {
        base.Init();
        data = SkillDataManager.Instance.GetPassiveSkillData(GetType().Name);
    }
}