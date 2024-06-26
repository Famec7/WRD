using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkillDataBase", menuName = "Scriptable Object/Skill/ActiveSkillDataBase")]
public class ActiveSkillDataBase : ScriptableObject
{
    private List<ActiveSkillData> _activeSkillDataList;
    
    [ContextMenu("Load")]
    public void Load()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("DataTable/Skill/ActiveSkillData");
        _activeSkillDataList = new List<ActiveSkillData>(csvData.Count);

        for (int i = 0; i < csvData.Count; i++)
        {
            ActiveSkillData activeSkillData = new ActiveSkillData();
            activeSkillData.name = (csvData[i]["skill_name"].ToString());
            activeSkillData.values = new();

            var values = csvData[i]["skill_value"].ToString().Split(',');
            foreach (var value in values)
            {
                activeSkillData.values.Add(int.TryParse(value, out int result) ? result : 0);
            }

            activeSkillData.coolTime = float.Parse(csvData[i]["skill_cooltime"].ToString());
            _activeSkillDataList.Add(activeSkillData);
        }
    }
    
    /// <summary>
    /// 액티브 스킬 데이터 반환
    /// </summary>
    /// <param name="name"> 스킬 이름 </param>
    /// <returns> 스킬 데이터 반환 (이름이랑 일치하는 스킬 없으면 null) </returns>
    public ActiveSkillData GetActiveSkillData(string name)
    {
        foreach (var data in _activeSkillDataList)
        {
            if (data.name == name)
            {
                return data;
            }
        }

        return null;
    }
}

public class ActiveSkillData
{
    public string name; // 스킬 이름
    public List<int> values; // 스킬 데이터 관련된 값들
    public float coolTime; // 스킬 쿨타임
}