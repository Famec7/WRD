using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillDatabase", menuName = "Scriptable Object/Skill/PassiveSkillDatabase",
    order = 1)]
public class PassiveSkillDataBase : ScriptableObject
{
    private List<PassiveSkillData> _passiveSkillDataList;
    private List<PassiveAuraSkillData> _passiveAuraSkillDataList;

    [ContextMenu("Load")]
    public void Load()
    {
        var csvData = CSVReader.Read("DataTable/Skill/PassiveSkillData");
        _passiveSkillDataList = new List<PassiveSkillData>(csvData.Count);
        _passiveAuraSkillDataList = new List<PassiveAuraSkillData>(csvData.Count);

        foreach (var data in csvData)
        {
            // 패시브 스킬
            if (string.Compare(data["skill_type_1"].ToString(), "p", StringComparison.Ordinal) == 0)
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData
                {
                    ID = int.Parse(data["num"].ToString()),
                    Name = (data["skill_name"].ToString()), Chance = int.Parse(data["skill_chance"].ToString()),
                };

                /****************Range Parse****************/
                if (float.TryParse(data["area"].ToString(), out var result))
                {
                    passiveSkillData.Range = result;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"{passiveSkillData.Name} {data["area"].ToString()} can't parse");
#endif
                }

                /****************Value Parse****************/
                var values = data["skill_value"].ToString().Split(',');

                foreach (var value in values)
                {
                    if (float.TryParse(value, out result))
                    {
                        passiveSkillData.AddValue(result);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogWarning($"{passiveSkillData.Name} {value} can't parse");
#endif
                    }
                }

                _passiveSkillDataList.Add(passiveSkillData);
            }
            // 패시브 오라 스킬
            else
            {
                PassiveAuraSkillData passiveAuraSkillDataData = new PassiveAuraSkillData
                {
                    ID = int.Parse(data["num"].ToString()),
                    Name = (data["skill_name"].ToString()),
                };

                /****************Range Parse****************/
                if (float.TryParse(data["area"].ToString(), out var result))
                {
                    passiveAuraSkillDataData.Range = result;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"{passiveAuraSkillDataData.Name} {data["area"].ToString()} can't parse");
#endif
                }

                /****************Value Parse****************/
                var values = data["skill_value"].ToString().Split(',');

                foreach (var value in values)
                {
                    if (float.TryParse(value, out result))
                    {
                        passiveAuraSkillDataData.AddValue(result);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogWarning($"{passiveAuraSkillDataData.Name} {value} can't parse");
#endif
                    }
                }

                _passiveAuraSkillDataList.Add(passiveAuraSkillDataData);
            }
        }
    }

    /// <summary>
    /// 패시브 스킬 데이터 반환
    /// </summary>
    /// <param name="id"> 스킬 id </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public PassiveSkillData GetPassiveSkillData(int id)
    {
        foreach (var data in _passiveSkillDataList)
        {
            if (data.ID == id)
            {
                return data;
            }
        }

#if UNITY_EDITOR
        Debug.LogError($"Not found PassiveSkill's {id} data");
#endif

        return null;
    }

    /// <summary>
    /// 패시브 오라 스킬 데이터 반환
    /// </summary>
    /// <param name="id"> 스킬 id </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public PassiveAuraSkillData GetPassiveAuraSkillData(int id)
    {
        foreach (var data in _passiveAuraSkillDataList)
        {
            if (data.ID == id)
            {
                return data;
            }
        }

#if UNITY_EDITOR
        Debug.LogError($"Not found PassiveAuraSkill's {id} data");
#endif

        return null;
    }
}