public class SkillDataManager : Singleton<SkillDataManager>
{
    private PassiveSkillDataBase passiveDataBase;
    private ActiveSkillDataBase activeDataBase;
    protected override void Init()
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
}