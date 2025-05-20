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
        MissionNum.text = (_index + 1).ToString();
    }

    protected override void SetClosePopUp()
    {
        base.SetClosePopUp();
    }

    public void ClickChallengeButton()
    {
        if (MissionManager.Instance.IsRunning)
        {
            return;
        }
        
        // 몬스터 생성
        Monster monster = MonsterSpawnManager.instance.SpawnMonster(UnitCode.MISSIONBOSS1 + _index);
        MissionManager.Instance.TargetMonsterList.Add(monster);

        // 타이머 시작
        MissionTimer missionTimer = MissionManager.Instance.StartTimer(_index, monster);

        // 몬스터와 타이머를 매핑
        MissionManager.Instance.MonsterTimerDict.Add(monster, missionTimer);

        UIManager.instance.ClosePopUp();
    }
}
