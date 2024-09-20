using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SettingManager;

public class MissionManager : Singleton<MissionManager>
{
    public GameObject MissionTimerPrefab;
    public List<Monster> TargetMonsterList;
    public List<MissionTimer> MissionTimerList;
    protected override void Init()
    {
        ;
    }

    public void StartTimer()
    {

    }
}
