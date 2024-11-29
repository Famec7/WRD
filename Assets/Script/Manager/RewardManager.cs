using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

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
        if (code >= UnitCode.MISSIONBOSS1 && code <= UnitCode.MISSIONBOSS6)
        {
            int idx = code - UnitCode.MISSIONBOSS1;
            Tuple<string, List<int>> rewardTuple = MissionMonsterManager.instance.rewardList[idx];

            string[] rewardStr = rewardTuple.Item1.Split(',');

            for (int i = 0; i < rewardStr.Length; i++)
            {
                if (rewardStr[i].EndsWith("_m"))
                {
                    if(Enum.TryParse(rewardStr[i].Replace("_m", ""), true, out WeaponTier tier))
                    {
                        MasterKeyManager.Instance.UpdateMasterKeyCount(tier, rewardTuple.Item2[i]);
                    }
                }
                else
                {
                    if (Enum.TryParse(rewardStr[i], true, out WeaponTier tier))
                    {
                        GetRandomWeapon(tier, rewardTuple.Item1[i]);
                    }
                }
            }
        }    
    }

    public void GetRandomWeapon(WeaponTier tier,int count)
    {
        for (int i = 0; i < count; i++)
        {
            List<WeaponData> sameTierWeaponList = WeaponDataManager.Instance.Database.GetAllSameTierWeaponData(tier);
            int random = Random.Range(0, sameTierWeaponList.Count);
            WeaponData rewardData = sameTierWeaponList[random];
            InventoryManager.instance.AddItemByNum(rewardData.num);
        }
    }



}
