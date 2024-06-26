using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillDatabase", menuName = "Scriptable Object/Skill/PassiveSkillDatabase",
    order = 1)]
public class PassiveSkillDataBase : ScriptableObject
{
    private List<PassiveSkillData> _passiveSkillDataList;
    private List<PassiveSkillData> _passiveAuraSkillDataList;

    [ContextMenu("Load")]
    public void Load()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("DataTable/Skill/PassiveSkillData");
        _passiveSkillDataList = new List<PassiveSkillData>(csvData.Count);
        _passiveAuraSkillDataList = new List<PassiveSkillData>(csvData.Count);

        for (int i = 0; i < csvData.Count; i++)
        {
            // 패시브 스킬
            if (String.Compare(csvData[i]["skill_type_1"].ToString(), "p", StringComparison.Ordinal) == 0)
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData
                {
                    name = (csvData[i]["skill_name"].ToString()),
                    values = new()
                };

                var values = csvData[i]["skill_value"].ToString().Split(',');
                foreach (var value in values)
                {
                    passiveSkillData.values.Add(int.TryParse(value, out int result) ? result : 0);
                }

                _passiveSkillDataList.Add(passiveSkillData);
            }
            // 패시브 오라 스킬
            else
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData
                {
                    name = (csvData[i]["skill_name"].ToString()),
                    values = new()
                };

                var values = csvData[i]["skill_value"].ToString().Split(',');
                foreach (var value in values)
                {
                    passiveSkillData.values.Add(int.TryParse(value, out int result) ? result : 0);
                }

                _passiveAuraSkillDataList.Add(passiveSkillData);
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
            if (data.name == skillName)
            {
                return data;
            }
        }

        return null;
    }

    /// <summary>
    /// 패시브 오라 스킬 데이터 반환
    /// </summary>
    /// <param name="name"> 스킬 이름 </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public PassiveSkillData GetPassiveAuraSkillData(string name)
    {
        foreach (var data in _passiveAuraSkillDataList)
        {
            if (data.name == name)
            {
                return data;
            }
        }

        return null;
    }
}

[Serializable]
public class PassiveSkillData
{
    public string name; // 스킬 이름
    public List<int> values; // 스킬 데이터 관련된 값
}