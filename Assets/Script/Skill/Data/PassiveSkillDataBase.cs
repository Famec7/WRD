using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillDatabase", menuName = "Scriptable Object/Skill/PassiveSkillDatabase", order = 1)]
public class PassiveSkillDataBase : ScriptableObject
{
    private List<PassiveSkillData> _passiveSkillDataList;
    private List<PassiveSkillData> _passiveAuraSkillDataList;

    [ContextMenu("Load")]
    public void Load()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("PassiveSkill");
        _passiveSkillDataList = new List<PassiveSkillData>(csvData.Count);

        for (int i = 0; i < csvData.Count; i++)
        {
            if (String.Compare(csvData[i]["skill_type_2"].ToString(), "p", StringComparison.Ordinal) == 0)
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData();
                passiveSkillData.name = (csvData[i]["skill_name"].ToString());
                passiveSkillData.values = new();

                var values = csvData[i]["skill_value"].ToString().Split(',');
                foreach (var value in values)
                {
                    passiveSkillData.values.Add(int.Parse(value));
                }

                _passiveSkillDataList.Add(passiveSkillData);
            }
            else
            {
                PassiveSkillData passiveSkillData = new PassiveSkillData();
                passiveSkillData.name = (csvData[i]["skill_name"].ToString());
                passiveSkillData.values = new();

                var values = csvData[i]["skill_value"].ToString().Split(',');
                foreach (var value in values)
                {
                    passiveSkillData.values.Add(int.Parse(value));
                }

                _passiveAuraSkillDataList.Add(passiveSkillData);
            
            }
        }
    }

    public PassiveSkillData GetPassiveSkillData(string name)
    {
        foreach (var data in _passiveSkillDataList)
        {
            if (data.name == name)
            {
                return data;
            }
        }

        return null;
    }
    
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
    public string name;
    public List<int> values;
}