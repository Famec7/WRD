using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMonsterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MissionMonsterManager instance;

    public int[] defenseData;

    public UnitCode[] unitCodeData;

    public float[] speedData;
    public float[] resistData;
    public float[] HPData;
    public float[] playTimeData;
    public string[] monsterNameData;

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
        for (int i = 0; i < data.Count; i++)
        {
            defenseData[i] = int.Parse((data[i]["defense"]).ToString());
            speedData[i] = float.Parse((data[i]["speed"]).ToString());
            resistData[i] = float.Parse((data[i]["resist"]).ToString());
            HPData[i] = float.Parse((data[i]["hp"]).ToString());
            monsterNameData[i] = data[i]["monster_name"].ToString();
            playTimeData[i] = float.Parse((data[i]["mission_playtime"]).ToString());

            unitCodeData[i] = UnitCode.MISSIONBOSS1 + i;
        }
    }

    // Update is called once per frame
 
}
