using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    public GameObject MissionTimerPrefab;
    public List<Monster> TargetMonsterList;
    public List<MissionTimer> MissionTimerList;

    public Dictionary<Monster, MissionTimer> MonsterTimerDict = new Dictionary<Monster, MissionTimer>();
    public MissionInfo missionInfo;

    private int _timerCnt = 0;
    
    public bool IsRunning => TargetMonsterList.Count > 0;

    protected override void Init()
    {
        TargetMonsterList = new List<Monster>();
        MissionTimerList = new List<MissionTimer> { };
        MonsterTimerDict = new Dictionary<Monster, MissionTimer> { };
    }

    public MissionTimer StartTimer(int index, Monster monster)
    {
        GameObject missionTimerObject = Instantiate(MissionTimerPrefab,GameObject.Find("Canvas").transform);
        missionTimerObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -235 + ++_timerCnt * 25);
        missionTimerObject.transform.SetAsFirstSibling();
        
        MissionTimer missionTimer = missionTimerObject.GetComponent<MissionTimer>();
        MissionTimerList.Add(missionTimer);
        missionTimer.StartTimer(MissionMonsterManager.instance.playTimeData[index],index);

        missionTimer.OnTimerEnd += () => OnMissionTimeUp(monster);

        return missionTimer;
    }

    public void OnMissionTimeUp(Monster monster)
    {
        if (monster != null && !monster.isDead)
        {
            // 몬스터 제거 로직
            MessageManager.Instance.ShowMessage(monster.status.unitCode.ToString() + " Fail!", new Vector2(0, 200), 2f, 0.5f);
            MonsterPoolManager.Instance.ReturnObject(monster.status.unitCode.ToString(), monster.gameObject);
            MonsterHPBarPool.ReturnObject(monster.transform.GetChild(2).GetComponent<MonsterHPBar>());
            TargetMonsterList.Remove(monster);
            MonsterTimerDict.Remove(monster);
        }
    }
}
