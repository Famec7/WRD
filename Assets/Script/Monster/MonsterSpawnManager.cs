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
        UIManager.instance.currentWaveTime.text = wavePlayTime[0].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int idx = GameManager.instance.wave - 1;
        //Boss가 나오는 Wave인지 확인하는 bool 변수
        isBossWave = bossSpawnNum[idx] >= 1;

        if (idx >= 34)
            idx = 34;

        // 스폰 딜레이시간 세기
        spawnDelayTimer += Time.deltaTime;

        // 현재 몬스터 수가 총 몬스터 숫자를 넘으면 생성 중지
        if (currentMonsterNum >= limitMonsterNum)
            isNormalSpawnStop = true;
        else // 아닐 경우 웨이브 시간을 올려줌
            waveSecTimer += Time.deltaTime;

        if (waveSecTimer >= 1f) // 1초 단위로 웨이브 시간 텍스트 변경
        {
            waveTimer++;
            UIManager.instance.currentWaveTime.text = (wavePlayTime[idx] - waveTimer).ToString();
            waveSecTimer = 0f;
        }
        // 웨이브 시간이 끝나면
        if (waveTimer >= wavePlayTime[idx])
        {
            //제한 몬스터 수 계산 
            int limitMonsterNum = 80;

            if (GameManager.instance.wave >= 11 && GameManager.instance.wave <= 20)
                limitMonsterNum = 70;
            if (GameManager.instance.wave >= 21 && GameManager.instance.wave <= 30)
                limitMonsterNum = 60;
            if (GameManager.instance.wave >= 31 && GameManager.instance.wave <= 35)
                limitMonsterNum = 50;

            UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();

            // 보스 웨이브인데 보스를 잡지 못했을 경우 or 현재 몬스터보다 제한 몬스터가 많을 경우 GameOver
            if ((isBossWave && targetBossStatus.HP < 0) && currentMonsterNum > limitMonsterNum)
            {
                GameManager.instance.isGameOver = true;
            }
            else if (isBossWave) // 보스 보상 지급
            {
                Debug.Log("보상 지급");
            }
            // 웨이브 및 텍스트 업데이트
            GameManager.instance.wave++;
            UIManager.instance.waveNum.text = GameManager.instance.wave.ToString();
            // 모든 스테이지 기본 보상 3개 지급
            ElementManager.instance.GetElement(3);
            // 다시 생성을 위한 변수 초기화
            waveTimer = 0;
            isNormalSpawnStop = false;
            currentWaveMonsterNum = 0;
            isSpawnStop = false;
            isBossSpawn = false;
            idx = GameManager.instance.wave - 1;
            isBossWave = bossSpawnNum[idx] >= 1;
        }

        if (isSpawnStop) return;

        // 일반 몬스터 스폰 멈추기
        if (currentWaveMonsterNum == monsterSpawnNum[idx])
            isNormalSpawnStop = true;

        // 일반 몬스터 소환
        if (spawnDelayTimer >= monsterSpawnTime[idx] && !isNormalSpawnStop)
        {
            UnitCode code = (UnitCode)((int)GameManager.instance.wave / 6);
            SpawnMonster(code);
        }

        // 보스 몬스터 소환
        if (isBossWave && !isBossSpawn)
        {
            UnitCode code = (UnitCode)((int)GameManager.instance.wave / 6) + 6;
            SpawnMonster(code);
            isBossSpawn = true;
        }
        // WAVE SKIP
        if ((!isBossWave && isNormalSpawnStop) || (isBossWave && targetBossStatus.HP <= 0))
        {
            if (GameManager.instance.isSKip || isBossWave)
            {
                waveTimer = wavePlayTime[idx];
                isNormalSpawnStop = false;
                isSpawnStop = true;
            }
        }
    }
    
    void SpawnMonster(UnitCode code)
    {
        GameObject monster = MonsterPool.instance.GetPooledObject(code);
        monster.GetComponent<MonsterMoveComponent>().roadNum = 1; // *임시코드* 생성 위치 바꾸면 코드 수정해야함
        monster.transform.position = spawnPoints[spawnPointsCount - 1].transform.position;

        GameObject hpBar = Instantiate(hpBarPrefab, GameObject.Find("Canvas").transform);
        hpBar.GetComponent<MonsterHPBar>().owner = monster;
        hpBar.GetComponent<MonsterHPBar>().ownerStatus = monster.GetComponent<Status>();
        hpBar.transform.SetAsFirstSibling();
        hpBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(monster.transform.position.x, monster.transform.position.y + 0.1f, 0));

        spawnDelayTimer = 0;
        currentMonsterNum++;
        currentWaveMonsterNum++;

        UIManager.instance.currentMonsterNum.text = currentMonsterNum.ToString();
        UnitManager.instance.monsterList.Add(monster);
    }


    public void waveSkip()
    {
        if (currentWaveMonsterNum == monsterSpawnNum[GameManager.instance.wave - 1])
        {
            waveTimer = wavePlayTime[GameManager.instance.wave - 1];
            isSpawnStop = false;
        }
    }
}
