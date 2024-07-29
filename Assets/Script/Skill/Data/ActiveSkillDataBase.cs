using System;
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
            
            /************cooltime Parse*************/
            if(float.TryParse(data["skill_cooltime"].ToString(), out var coolTime))
            {
                activeSkillData.CoolTime = coolTime;
            }
            else
            {
                Debug.LogError("${activeSkillData.Name}'s cooltime is not float");
            }
            
            /****************active type Parse****************/
            if(Enum.TryParse(data["skill_type"].ToString(), out ActiveSkillData.ActiveType activeType))
            {
                activeSkillData.Type = activeType;
            }
            else
            {
                Debug.LogError($"{activeSkillData.Name}'s type is not ActiveType");
            }
            
            /****************skill available range Parse****************/
            if(float.TryParse(data["range"].ToString(), out var range))
            {
                activeSkillData.AvailableRange = range;
            }
            else
            {
                Debug.LogError($"{activeSkillData.Name}'s range is not float");
            }
            
            /****************skill range Parse****************/
            if(float.TryParse(data["area"].ToString(), out var skillRange))
            {
                activeSkillData.Range = skillRange;
            }
            else
            {
                Debug.LogError($"{activeSkillData.Name}'s skill range is not float");
            }

            /****************skill value Parse****************/
            var values = data["skill_value"].ToString().Split(',');
            foreach (var value in values)
            {
                if(float.TryParse(value, out var result))
                {
                    activeSkillData.AddValue(result);
                }
                else
                {
                    Debug.LogError($"{activeSkillData.Name}'s value is not float");
                }
            }
            
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
            if (string.Compare(data.Name.Replace(" ",""), skillName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return data;
            }
        }

        return null;
    }
}