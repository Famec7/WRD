public class SkillDataManager : Singleton<SkillDataManager>
{
    private SkillDataBase passiveDataBase;
    private SkillDataBase activeDataBase;
    private Skill _currentSkill;
    
    protected override void Init()
    {
        passiveDataBase = ResourceManager.Instance.Load<SkillDataBase>("Database/PassiveDatabase");
    }
    
    public SkillData GetPassiveSkillData(string skillName)
    {
        return passiveDataBase.skillDataList.Find(x => x.skillName == skillName);
    }
    public SkillData GetActiveSkillData(string skillName)
    {
        return activeDataBase.skillDataList.Find(x => x.skillName == skillName);
    }
    
    public SkillData GetPassiveSkillData(int skillNumber)
    {
        return passiveDataBase.GetSkillData(skillNumber);
    }
    
    public SkillData GetActiveSkillData(int skillNumber)
    {
        return activeDataBase.GetSkillData(skillNumber);
    }
}