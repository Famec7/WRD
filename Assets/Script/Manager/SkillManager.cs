public class SkillManager : Singleton<SkillManager>
{
    protected override void Init()
    {
        InitData();
    }

    #region Data

    private PassiveSkillDataBase passiveDataBase;
    private ActiveSkillDataBase activeDataBase;

    private void InitData()
    {
        passiveDataBase = ResourceManager.Instance.Load<PassiveSkillDataBase>("Database/Skill/PassiveSkillDataBase");
        activeDataBase = ResourceManager.Instance.Load<ActiveSkillDataBase>("Database/Skill/ActiveSkillDataBase");
        
        passiveDataBase.Load();
        /*activeDataBase.Load();*/
    }
    
    public PassiveSkillData GetPassiveSkillData(string skillName)
    {
        return passiveDataBase.GetPassiveSkillData(skillName);
    }
    
    public PassiveAuraSkillData GetPassiveAuraSkillData(string skillName)
    {
        return passiveDataBase.GetPassiveAuraSkillData(skillName);
    }

    #endregion

    #region Active Skill

    private ActiveSkillBase _currentActiveSkill = null;
    
    public void SetActiveSkill(ActiveSkillBase skill)
    {
        _currentActiveSkill = skill;
    }

    #endregion
}