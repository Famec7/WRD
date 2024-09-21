using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public static MonsterSpawnManager instance;

    [HideInInspector]
    public bool isNormalSpawnStop = false;

    public bool isBossWave = false;

    public float spawnDelayTimer = 0f;
    public float maxSpawnDelay = 2f;

    public float waveTimer = 0f;
    float waveSecTimer = 0f;

    public GameObject[] spawnPoints;
    public GameObject hpBarPrefab;
    public Status targetBossStatus;

    public int spawnPointsCount = 1;
    public int currentMonsterNum = 0;
    public int currentWaveMonsterNum = 0;
    public int limitMonsterNum = 80;

    int[] waveNum;
    int[] monsterSpawnNum;
    int[] bossSpawnNum;
    int[] wavePlayTime;

    float[] monsterSpawnTime;

    bool isBossSpawn = false;
    bool isSpawnStop = false;
    
    [SerializeField]
    private Canvas _hpBarCanvas;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("WaveInfo");

        waveNum = new int[data.Count];
        monsterSpawnNum = new int[data.Count];
        bossSpawnNum = new int[data.Count];
        wavePlayTime = new int[data.Count];
        monsterSpawnTime = new float[data.Count];


        for (int i = 0; i < data.Count; i++)
        {
            waveNum[i] = int.Parse(data[i]["wave_num"].ToString());
            monsterSpawnNum[i] = int.Parse(data[i]["monster_spawnnum"].ToString());
            bossSpawnNum[i] = int.Parse(data[i]["boss_spawnnum"].ToString());
            wavePlayTime[i] = int.Parse(data[i]["wave_playtime"].ToString());
            monsterSpawnTime[i] = float.Parse((data[i]["moster_spawntme"]).ToString());
        }


        if (limitMonsterNum >= 11 && limitMonsterNum <= 20)
            limitMonsterNum = 70;
        if (limitMonsterNum >= 21 && limitMonsterNum <= 30)
            limitMonsterNum = 60;
        if (limitMonsterNum >= 31 && limitMonsterNum <= 35)
            limitMonsterNum = 50;

        UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();
        TimeSpan timeSpan = TimeSpan.FromSeconds(wavePlayTime[0]);
        UIManager.instance.currentWaveTime.text = timeSpan.ToString(@"mm\:ss"); 
    }

    // Update is called once per frame
    void Update()
    {
        int idx = GameManager.Instance.wave - 1;
        isBossWave = bossSpawnNum[idx] >= 1;

        if (idx >= 34)
            idx = 34;

        spawnDelayTimer += Time.deltaTime;

        if (currentMonsterNum >= limitMonsterNum)
            isNormalSpawnStop = true;
        else
            waveSecTimer += Time.deltaTime;

        if (waveSecTimer >= 1f) 
        {
            waveTimer++;
            TimeSpan timeSpan = TimeSpan.FromSeconds(wavePlayTime[idx]-waveTimer);
            UIManager.instance.currentWaveTime.text = timeSpan.ToString(@"mm\:ss");
            waveSecTimer = 0f;
        }
        if (waveTimer >= wavePlayTime[idx])
        {
            int limitMonsterNum = 80;

            if (GameManager.Instance.wave >= 11 && GameManager.Instance.wave <= 20)
                limitMonsterNum = 70;
            if (GameManager.Instance.wave >= 21 && GameManager.Instance.wave <= 30)
                limitMonsterNum = 60;
            if (GameManager.Instance.wave >= 31 && GameManager.Instance.wave <= 35)
                limitMonsterNum = 50;

            UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();

            if ((isBossWave && targetBossStatus.HP < 0) && currentMonsterNum > limitMonsterNum)
            {
                GameManager.Instance.isGameOver = true;
            }
            else if (isBossWave) // ���� ���� ����
            {
                Debug.Log("���� ����");
            }
            GameManager.Instance.wave++;
            UIManager.instance.waveNum.text = "Wave " + GameManager.Instance.wave.ToString();
            ElementManager.instance.GetElement(3);
            waveTimer = 0;
            isNormalSpawnStop = false;
            currentWaveMonsterNum = 0;
            isSpawnStop = false;
            isBossSpawn = false;
            idx = GameManager.Instance.wave - 1;
            isBossWave = bossSpawnNum[idx] >= 1;
        }

        if (isSpawnStop) return;

        if (currentWaveMonsterNum == monsterSpawnNum[idx])
            isNormalSpawnStop = true;

        if (spawnDelayTimer >= monsterSpawnTime[idx] && !isNormalSpawnStop)
        {
            UnitCode code = (UnitCode)((int)GameManager.Instance.wave / 6);
            SpawnMonster(code);
        }

        if (isBossWave && !isBossSpawn)
        {
            UnitCode code = (UnitCode)((int)GameManager.Instance.wave / 6) + 6;
            SpawnMonster(code);
            isBossSpawn = true;
        }
        // WAVE SKIP
        if ((!isBossWave && isNormalSpawnStop) || (isBossWave && targetBossStatus.HP <= 0))
        {
            if (GameManager.Instance.isSKip || isBossWave)
            {
                waveTimer = wavePlayTime[idx];
                isNormalSpawnStop = false;
                isSpawnStop = true;
            }
        }
    }
    
    public Monster SpawnMonster(UnitCode code)
    {
        var monster = MonsterPoolManager.Instance.GetPooledObject(code);
        monster.GetComponent<MonsterMoveComponent>().roadNum = 1; 
        monster.transform.position = spawnPoints[spawnPointsCount - 1].transform.position;

        GameObject hpBar = MonsterHPBarPool.GetObject();
        hpBar.transform.parent = monster.transform;
        hpBar.transform.position = monster.transform.position + new Vector3(0,0.2f);
        hpBar.SetActive(true);
        hpBar.GetComponent<MonsterHPBar>().owner = monster.gameObject;
        hpBar.GetComponent<MonsterHPBar>().ownerStatus = monster.GetComponent<Status>();

        spawnDelayTimer = 0;
        currentMonsterNum++;
        currentWaveMonsterNum++;

        UIManager.instance.currentMonsterNum.text = currentMonsterNum.ToString();
        UnitManager.instance.monsterList.Add(monster.gameObject);

        return monster;
    }


    public void waveSkip()
    {
        if (currentWaveMonsterNum == monsterSpawnNum[GameManager.Instance.wave - 1])
        {
            waveTimer = wavePlayTime[GameManager.Instance.wave - 1];
            isSpawnStop = false;
        }
    }
}
