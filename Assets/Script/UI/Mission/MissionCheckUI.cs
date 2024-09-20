using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionCheckUI : UIPopUp
{
    [SerializeField]
    private TextMeshProUGUI MissionNum;
    private int _index;

    public int Index
    {
        set => _index = value;
    }
    public override void Init()
    {
        MissionNum.text = (_index+1).ToString();
    }
    protected override void SetClosePopUp()
    {
        base.SetClosePopUp();
    }

    public void ClickChallengeButton()
    {
        MonsterSpawnManager.instance.SpawnMonster(UnitCode.MISSIONBOSS1 + _index);
        UIManager.instance.ClosePopUp();
    }
}
