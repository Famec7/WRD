using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillDataBase", menuName = "Scriptable Object/Skill/ActiveSkillDataBase")]
public class ActiveSkillDataBase : ScriptableObject
{
    private List<ActiveSkillData> _activeSkillDataList;
    
    [ContextMenu("Load")]
    public void Load()
    {
        var csvData = CSVReader.Read("DataTable/Skill/ActiveSkillData");
        _activeSkillDataList = new List<ActiveSkillData>(csvData.Count);

        foreach (var data in csvData)
        {
            ActiveSkillData activeSkillData = new ActiveSkillData
            {
                Name = (data["skill_name"].ToString())
            };

            var values = data["skill_value"].ToString().Split(',');
            foreach (var value in values)
            {
                if(int.TryParse(value, out var result))
                {
                    activeSkillData.AddValue(result);
                }
                else
                {
                    Debug.LogError("Value is not integer");
                }
            }

            activeSkillData.CoolTime = float.Parse(data["skill_cooltime"].ToString());
            _activeSkillDataList.Add(activeSkillData);
        }
    }
    
    /// <summary>
    /// 액티브 스킬 데이터 반환
    /// </summary>
    /// <param name="skillName"> 스킬 이름 </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public ActiveSkillData GetActiveSkillData(string skillName)
    {
        foreach (var data in _activeSkillDataList)
        {
            if (data.Name == skillName)
            {
                return data;
            }
        }

        return null;
    }
}