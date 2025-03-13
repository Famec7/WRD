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
                Name = (data["skill_name"].ToString()), ID = int.Parse(data["num"].ToString())
            };

            /************cooltime Parse*************/
            if (float.TryParse(data["skill_cooltime"].ToString(), out var coolTime))
            {
                activeSkillData.CoolTime = coolTime;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{activeSkillData.Name}'s cooltime is not float");
#endif
            }

            /****************active type Parse****************/
            if (Enum.TryParse(data["skill_activation_type"].ToString(), out ActiveSkillData.ActiveType activeType))
            {
                activeSkillData.Type = activeType;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{activeSkillData.Name}'s type is not ActiveType");
#endif
            }

            /****************skill available range Parse****************/
            if (float.TryParse(data["range"].ToString(), out var range))
            {
                activeSkillData.AvailableRange = range;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{activeSkillData.Name}'s range is not float");
#endif
            }

            /****************skill range Parse****************/
            if (float.TryParse(data["area"].ToString(), out var skillRange))
            {
                activeSkillData.Range = skillRange;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{activeSkillData.Name}'s skill range is not float");
#endif
            }
            
            /****************skill short info Parse****************/
            activeSkillData.Description = data["skill_shortinfo"].ToString();

            /****************skill value Parse****************/
            var values = data["skill_value"].ToString().Split(',');
            foreach (var value in values)
            {
                if (float.TryParse(value, out var result))
                {
                    activeSkillData.AddValue(result);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"{activeSkillData.Name}'s value is not float");
#endif
                }
            }

            _activeSkillDataList.Add(activeSkillData);
        }
    }

    /// <summary>
    /// 액티브 스킬 데이터 반환
    /// </summary>
    /// <param name="id"> 스킬 id </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public ActiveSkillData GetActiveSkillData(int id)
    {
        foreach (var data in _activeSkillDataList)
        {
            if (data.ID == id)
            {
                return data;
            }
        }

#if UNITY_EDITOR
        Debug.LogError($"Not found ActiveSkill's {id} data");
#endif

        return null;
    }
}