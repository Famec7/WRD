using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    // Start is called before the first frame update


    public int[] elementRewardCnt;
    public int[] unnormalRewardCnt;
    public int[] rareRewardCnt;
    public int[] epicRewardCnt;
    public int[] legendaryRewardCnt;
    public int[] mythRewardCnt;

    protected override void Init()
    {
        ;
    }
    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("WaveReward");

        elementRewardCnt = new int[data.Count];
        unnormalRewardCnt = new int[data.Count];
        rareRewardCnt = new int[data.Count];
        epicRewardCnt = new int[data.Count];
        legendaryRewardCnt = new int[data.Count];
        mythRewardCnt = new int[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            elementRewardCnt[i] = int.Parse(data[i]["element"].ToString());
            unnormalRewardCnt[i] = int.Parse(data[i]["unnormal"].ToString());
            rareRewardCnt[i] = int.Parse(data[i]["rare"].ToString());
            epicRewardCnt[i] = int.Parse(data[i]["epic"].ToString());
            legendaryRewardCnt[i] = int.Parse((data[i]["legendary"]).ToString());
            mythRewardCnt[i] = int.Parse((data[i]["myth"]).ToString());
        }
    }


    public void GetReward(int wave)
    {
        if (elementRewardCnt[wave] > 0)
        {
            ElementManager.instance.GetElement(elementRewardCnt[wave]);

        }
    }

    public void GetReward(UnitCode code)
    {
          ElementManager.instance.GetElement(3);
    }
}
