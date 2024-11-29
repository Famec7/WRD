using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionMonsterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MissionMonsterManager instance;

    public int[] defenseData;
    public List<Tuple<string, List<int>>> rewardList = new List<Tuple<string, List<int>>>();
    public UnitCode[] unitCodeData;

    public float[] speedData;
    public float[] resistData;
    public float[] HPData;
    public float[] playTimeData;
    public string[] monsterNameData;
    public string[] rewardGrades;
    public string[] rewardNumbers;

    private void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("MissionMonster");

        if (instance == null)
            instance = this;

        unitCodeData = new UnitCode[data.Count];
        defenseData = new int[data.Count];
        speedData = new float[data.Count];
        resistData = new float[data.Count];
        HPData = new float[data.Count];
        monsterNameData = new string[data.Count];
        playTimeData = new float[data.Count];
        rewardGrades = new string[data.Count];
        rewardNumbers = new string[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            defenseData[i] = int.Parse((data[i]["defense"]).ToString());
            speedData[i] = float.Parse((data[i]["speed"]).ToString());
            resistData[i] = float.Parse((data[i]["resist"]).ToString());
            HPData[i] = float.Parse((data[i]["hp"]).ToString());
            monsterNameData[i] = data[i]["monster_name"].ToString();
            playTimeData[i] = float.Parse((data[i]["mission_playtime"]).ToString());
            rewardGrades[i] = data[i]["reward_grade"].ToString();
            rewardNumbers[i] = data[i]["reward_number"].ToString();
            unitCodeData[i] = UnitCode.MISSIONBOSS1 + i;
        }

        for (int i = 0; i < rewardGrades.Length; i++)
        {
            string grade = rewardGrades[i];
            string[] rewards = rewardNumbers[i].Split(',');

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
            rewardList.Add(new Tuple<string, List<int>>(grade, rewardNumbersList));
        }

        // 결과 출력
        foreach (var item in rewardList)
        {
            Debug.Log($"Grade: {item.Item1}, Rewards: {string.Join(", ", item.Item2)}");
        }
    }

    // Update is called once per frame
 
}
