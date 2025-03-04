using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public List<Tuple<string, List<int>>> bossRewardList = new List<Tuple<string, List<int>>>();

    public string[] bossRewardGrades;
    public string[] bossRewardNumbers;

    [SerializeField]
    private GameObject rewardPopUpUIObejct_;
    protected override void Init()
    {
        ;
    }
    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("WaveReward");

        List<Dictionary<string, object>> bossRewardData = CSVReader.Read("bossRewardTable");

        bossRewardGrades = new string[data.Count];
        bossRewardNumbers = new string[data.Count];
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

        for (int i =0; i < bossRewardData.Count; i++)
        {
            bossRewardGrades[i] = bossRewardData[i]["reward_grade"].ToString();
            bossRewardNumbers[i] = bossRewardData[i]["reward_number"].ToString();
        }

        for (int i = 0; i < bossRewardData.Count; i++)
        {
            string grade = bossRewardGrades[i];
            string[] rewards = bossRewardNumbers[i].Split(',');

            List<int> rewardNumbersList = new List<int>();

            // 보상을 정수로 변환
            foreach (string reward in rewards)
            {
                if (int.TryParse(reward.Trim(), out int parsedReward))
                {
                    rewardNumbersList.Add(parsedReward);
                }
                else
                {
                    Debug.LogWarning($"Warning: Failed to parse reward '{reward}' at index {i}.");
                }
            }

            // 등급과 보상을 리스트에 추가
            bossRewardList.Add(new Tuple<string, List<int>>(grade, rewardNumbersList));
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
        Tuple<string, List<int>> rewardTuple = null;
        rewardPopUpUIObejct_.SetActive(true);
        RewardPopUpUI rewardPopUpUI = rewardPopUpUIObejct_.GetComponent<RewardPopUpUI>(); 
        if (code >= UnitCode.MISSIONBOSS1 && code <= UnitCode.MISSIONBOSS6)
        {
            int idx = code - UnitCode.MISSIONBOSS1;
            rewardTuple = MissionMonsterManager.instance.rewardList[idx];
            rewardPopUpUI.SettingRewardPopUpUI(false, idx);
        }
        else if (code >= UnitCode.BOSS1 && code <= UnitCode.BOSS6)
        {
            int idx = code - UnitCode.BOSS1;
            rewardTuple = bossRewardList[idx];
            rewardPopUpUI.SettingRewardPopUpUI(true, idx);
        }

        if (rewardTuple != null)
        {
            ProcessReward(rewardTuple);
        }
    }

    private void ProcessReward(Tuple<string, List<int>> rewardTuple)
    {
        string[] rewardStrArr = rewardTuple.Item1.Split(',');
        
        Dictionary<WeaponTier, int> masterKeyRewards = new Dictionary<WeaponTier, int>();
        Dictionary<WeaponTier, int> weaponRewards = new Dictionary<WeaponTier, int>();

        RewardPopUpUI rewardPopUpUI = rewardPopUpUIObejct_.GetComponent<RewardPopUpUI>();

        for (int i = 0; i < rewardStrArr.Length; i++)
        {
            string rewardStr = rewardStrArr[i];
            int count = rewardTuple.Item2[i];

        
            if (rewardStr.EndsWith("_m"))
            {
                if (Enum.TryParse(rewardStr.Replace("_m", ""), true, out WeaponTier tier))
                {
                    MasterKeyManager.Instance.UpdateMasterKeyCount(tier, count);
                    if (masterKeyRewards.ContainsKey(tier))
                        masterKeyRewards[tier] += count;
                    else
                        masterKeyRewards[tier] = count;
                }
            }
            else
            {
                if (Enum.TryParse(rewardStr, true, out WeaponTier tier))
                {
                    GetRandomWeapon(tier, count);
                    if (weaponRewards.ContainsKey(tier))
                        weaponRewards[tier] += count;
                    else
                        weaponRewards[tier] = count;
                }
            }
        }

        foreach (var kv in masterKeyRewards)
        {
            Tuple<WeaponTier, int> masterKeyRewardTuple = new Tuple<WeaponTier, int>(kv.Key, kv.Value);
            rewardPopUpUI.CreateMasterKeyRewardSlot(masterKeyRewardTuple);
        }

        //// 무기 보상이 있다면 간단한 메시지로 표시 (필요에 따라 별도 슬롯 생성 가능)
        //if (weaponRewards.Count > 0)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var kv in weaponRewards)
        //    {
        //        sb.Append($"{kv.Key} 등급: {kv.Value}개 획득\n");
        //    }
        //    MessageManager.Instance.ShowMessage(sb.ToString(), new Vector2(0, 218), 1f, 0.5f);
        //}
    }

    public void GetRandomWeapon(WeaponTier tier,int count)
    {
        RewardPopUpUI rewardPopUpUI = rewardPopUpUIObejct_.GetComponent<RewardPopUpUI>();

        for (int i = 0; i < count; i++)
        {
            List<WeaponData> sameTierWeaponList = WeaponDataManager.Instance.Database.GetAllSameTierWeaponData(tier);
            int random = Random.Range(0, sameTierWeaponList.Count);
            WeaponData rewardData = sameTierWeaponList[random];
            InventoryManager.instance.AddItemByNum(rewardData.num);
            rewardPopUpUI.CreateRandomWeaponRewardSlot(rewardData.num);
        }
    }



}
