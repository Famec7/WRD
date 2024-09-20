using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionDataManager : Singleton<MissionDataManager>
{
    private MissionDatabase _missionDatabase;

    protected override void Init()
    {
        _missionDatabase = Resources.Load<MissionDatabase>("Database/MissionDatabase");
        _missionDatabase.Load();
    }
    

    public string GetName(int index)
    {
        return _missionDatabase.GetMissionData(index).monsterName;
    }

    public WeaponTuple<WeaponTier, int> GetRequiredGrade(int index)
    {
        return _missionDatabase.GetMissionData(index).requiredGrade;
    }

    public List<WeaponTuple<WeaponTier, int>> GetEasyRequiredGrade(int index)
    {
        return _missionDatabase.GetMissionData(index).easyRequiredGrade;
    }
}