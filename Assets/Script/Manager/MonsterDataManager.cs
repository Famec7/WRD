using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static MonsterDataManager instance;

    public int[] defenseData;

    public UnitCode[] unitCodeData;

    public float[] speedData;
    public float[] resistData;
    public float[] HPData;

    public string[] monsterNameData;

    private void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("WaveMonster");

        if (instance == null)
            instance = this;

        defenseData = new int[data.Count];
        unitCodeData = new UnitCode[data.Count];
        speedData = new float[data.Count];
        resistData = new float[data.Count];
        HPData = new float[data.Count];
        monsterNameData = new string[data.Count];

        for (int i = 0; i < data.Count; i++)
        {
            defenseData[i] = int.Parse((data[i]["defense"]).ToString());
            unitCodeData[i] = (UnitCode)int.Parse((data[i]["monsterCode"].ToString()));
            speedData[i] = float.Parse((data[i]["speed"]).ToString());
            resistData[i] = float.Parse((data[i]["resist"]).ToString());
            HPData[i] = float.Parse((data[i]["hp"]).ToString());
            monsterNameData[i] = data[i]["monsterName"].ToString();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
