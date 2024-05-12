public class SkillDataManager : Singleton<SkillDataManager>
{
    private PassiveSkillDataBase passiveDataBase;
    private ActiveSkillDataBase activeDataBase;
    private Skill _currentSkill;
    protected override void Init()
    {
        passiveDataBase = ResourceManager.Instance.Load<PassiveSkillDataBase>("Database/Skill/PassiveSkillDataBase");
        activeDataBase = ResourceManager.Instance.Load<ActiveSkillDataBase>("Database/Skill/ActiveSkillDataBase");
    }
    
    public PassiveSkillData GetPassiveSkillData(string name)
    {
        return passiveDataBase.GetPassiveSkillData(name);
    }
}