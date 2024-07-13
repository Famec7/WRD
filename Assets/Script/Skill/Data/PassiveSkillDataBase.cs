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
                    Name = (data["skill_name"].ToString()),
                    Chance = int.Parse(data["skill_chance"].ToString()),
                };
                
                /****************Range Parse****************/
                if (float.TryParse(data["range"].ToString(), out var result))
                {
                    passiveSkillData.Range = result;
                }
                else
                {
                    Debug.LogError($"{passiveSkillData.Name} {data["range"].ToString()} can't parse");
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
                        Debug.LogError($"{passiveSkillData.Name} {value} can't parse");
                    }
                }

                _passiveSkillDataList.Add(passiveSkillData);
            }
            // 패시브 오라 스킬
            else
            {
                PassiveAuraSkillData passiveAuraSkillDataData = new PassiveAuraSkillData
                {
                    Name = (data["skill_name"].ToString()),
                };
                
                /****************Range Parse****************/
                if(float.TryParse(data["range"].ToString(), out var result))
                {
                    passiveAuraSkillDataData.Range = result;
                }
                else
                {
                    Debug.LogError($"{passiveAuraSkillDataData.Name} {data["range"].ToString()} can't parse");
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
                        Debug.LogError($"{passiveAuraSkillDataData.Name} {value} can't parse");
                    }
                }

                _passiveAuraSkillDataList.Add(passiveAuraSkillDataData);
            }
        }
    }

    /// <summary>
    /// 패시브 스킬 데이터 반환
    /// </summary>
    /// <param name="skillName"> 스킬 이름 </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public PassiveSkillData GetPassiveSkillData(string skillName)
    {
        foreach (var data in _passiveSkillDataList)
        {
            // 대소문자, 공백 구분 없이 비교
            if (string.Compare(data.Name.Replace(" ",""), skillName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return data;
            }
        }

        Debug.LogError($"Not found {skillName} data");
        return null;
    }

    /// <summary>
    /// 패시브 오라 스킬 데이터 반환
    /// </summary>
    /// <param name="skillName"> 스킬 이름 </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public PassiveAuraSkillData GetPassiveAuraSkillData(string skillName)
    {
        foreach (var data in _passiveAuraSkillDataList)
        {
            // 대소문자, 공백 구분 없이 비교
            if (string.Compare(data.Name.Replace(" ",""), skillName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return data;
            }
        }

        Debug.LogError($"Not found {skillName} data");
        return null;
    }
}