using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Scriptable Object/MissionDatabase", order = 0)]
public class MissionDatabase : ScriptableObject
{
    private List<MissionData> _missionDataList;
    
    [ContextMenu("Load MissionData")]
    public void Load()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("DataTable/MissionData");

        _missionDataList = new();
        for (int i = 0; i < data.Count; i++)
        {
            _missionDataList.Add(new MissionData());
            _missionDataList[i].easyRequiredGrade = new();

            _missionDataList[i].monsterName = data[i]["monster_name"].ToString();
            _missionDataList[i].requiredGrade = new WeaponTuple<WeaponTier, int>((WeaponTier)Enum.Parse(typeof(WeaponTier),data[i]["require_grade"].ToString(), true),
                int.Parse(data[i]["require_number"].ToString()));
            
            var easyGrades = data[i]["easy_grade"].ToString().Split(',');
            var easyNumbers = data[i]["easy_number"].ToString().Split(',');
            for (int j = 0; j < easyGrades.Length; j++)
            {
                string easyGradeName = easyGrades[j];
                _missionDataList[i].easyRequiredGrade
                    .Add(new WeaponTuple<WeaponTier, int>((WeaponTier)Enum.Parse(typeof(WeaponTier), easyGradeName, true),
                        int.Parse(easyNumbers[j])));
            }
        }
    }

    public MissionData GetMissionData(int index)
    {
        if (index < 0 || index > _missionDataList.Count)
        {
            Debug.LogError("Index out of range");
            return null;
        }

        return _missionDataList[index];
    }
}

[Serializable]
public struct WeaponTuple<T1, T2>
{
    public T1 grade;
    public T2 count;

    public WeaponTuple(T1 grade, T2 count)
    {
        this.grade = grade;
        this.count = count;
    }
}

[Serializable]
public class MissionData
{
    public string monsterName;
    public WeaponTuple<WeaponTier, int> requiredGrade;
    public List<WeaponTuple<WeaponTier, int>> easyRequiredGrade;
}