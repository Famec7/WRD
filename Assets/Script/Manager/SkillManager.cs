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
    
    public PassiveSkillData GetPassiveSkillData(int skillId)
    {
        return passiveDataBase.GetPassiveSkillData(skillId);
    }
    
    public PassiveAuraSkillData GetPassiveAuraSkillData(int skillId)
    {
        return passiveDataBase.GetPassiveAuraSkillData(skillId);
    }
    
    public ActiveSkillData GetActiveSkillData(int skillId)
    {
        return activeDataBase.GetActiveSkillData(skillId);
    }

    #endregion

    #region Skill

    private readonly List<SkillBase> _currentActiveSkill = new();
    
    private void AddSkill(SkillBase skill)
    {
        _currentActiveSkill.Add(skill);
        SkillUIManager.Instance.AddSkillButton(skill);
    }

    public void AddSkill(WeaponBase weapon)
    {
        for (int i = 0; i < weapon.GetActiveSkillCount(); i++)
        {
            ActiveSkillBase activeSkill = weapon.GetActiveSkill(i);
            AddSkill(activeSkill);
        }
        
        for (int i = 0; i < weapon.GetPassiveSkillCount(); i++)
        {
            PassiveSkillBase passiveSkill = weapon.GetPassiveSkill(i);
            AddSkill(passiveSkill);
        }
        
        for (int i = 0; i < weapon.GetPassiveAuraSkillCount(); i++)
        {
            PassiveAuraSkillBase passiveAuraSkill = weapon.GetPassiveAuraSkill(i);
            AddSkill(passiveAuraSkill);
        }
    }
    
    public void RemoveSkill(SkillBase skill)
    {
        _currentActiveSkill.Remove(skill);
        SkillUIManager.Instance.RemoveSkillButton(skill);
    }
    
    public void RemoveAllSkill()
    {
        foreach (var skill in _currentActiveSkill)
        {
            SkillUIManager.Instance.RemoveSkillButton(skill);
        }
        
        _currentActiveSkill.Clear();
    }
    
    public SkillBase GetActiveSkill(int index)
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