using System.Collections.Generic;
using UnityEngine;

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
        activeDataBase.Load();
    }
    
    public PassiveSkillData GetPassiveSkillData(string skillName)
    {
        return passiveDataBase.GetPassiveSkillData(skillName);
    }
    
    public PassiveAuraSkillData GetPassiveAuraSkillData(string skillName)
    {
        return passiveDataBase.GetPassiveAuraSkillData(skillName);
    }
    
    public ActiveSkillData GetActiveSkillData(string skillName)
    {
        return activeDataBase.GetActiveSkillData(skillName);
    }

    #endregion

    #region Active Skill

    private List<ActiveSkillBase> _currentActiveSkill = new();
    
    public void AddActiveSkill(ActiveSkillBase skill)
    {
        _currentActiveSkill.Add(skill);
        SkillUIManager.Instance.AddSkillButton(skill);
    }
    
    public void RemoveActiveSkill(ActiveSkillBase skill)
    {
        _currentActiveSkill.Remove(skill);
        SkillUIManager.Instance.RemoveSkillButton(skill);
    }
    
    public ActiveSkillBase GetActiveSkill(int index)
    {
        if (index < 0 || index >= _currentActiveSkill.Count)
        {
            Debug.LogError("$Active Skill Index Error: {index}");
            return null;
        }

        return _currentActiveSkill[index];
    }

    #endregion
}