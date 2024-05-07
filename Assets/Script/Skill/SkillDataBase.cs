using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Scriptable Object/SkillDatabase", order = int.MaxValue)]
public class SkillDataBase : ScriptableObject
{
    public List<SkillData> skillDataList = new List<SkillData>();

    /// <summary>
    /// 스킬 데이터를 CSV 파일에서 로드
    /// </summary>
    [ContextMenu("Load Skill Data From CSV")]
    public void Load()
    {
        ;
    }
    
    public SkillData GetSkillData(int skillNumber)
    {
        foreach (SkillData skillData in skillDataList)
        {
            if (skillData.skillNumber == skillNumber)
            {
                return skillData;
            }
        }

        Debug.LogError("해당 번호의 스킬 데이터가 없습니다.");
        return null;
    }
}

[Serializable]
public class SkillData
{
    public float projectileSpeed;
    public int skillNumber;
    public SkillType skillType; // 1: 기본 공격, 2: 패시브 스킬(공격시 발동), 3: 패시브 스킬(오라형), 4: 액티브 스킬
    public string skillName;
    public string skillDescription;
    public float skillDamage;
    public float skillRange;
    public float scopeRange;
    public float skillCooltime;
    public float checkCooltime;
    public bool canUse;
    public float skillUptime;
    public float checkUptime;
    public float skillChance;
    public Sprite skillIcon;
}