using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Monster targetBoss;


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

    public Button _skipButton;
    public bool isAutoWaveProgression = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _skipButton.onClick.AddListener(WaveSkip);
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

        UIManager.instance.waveNum.text = "Wave " + GameManager.Instance.wave.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        UIManager.instance.currentMonsterNum.text = currentMonsterNum.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1.0f;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2.0f;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 3.0f;

        if (GameManager.Instance.isGameOver) return;
        
        int idx = GameManager.Instance.wave - 1;
        isBossWave = bossSpawnNum[idx] >= 1;

        if (idx >= 34)
            idx = 34;

        spawnDelayTimer += Time.deltaTime;
        if(currentMonsterNum >= currentWaveMonsterNum)
            _skipButton.gameObject.SetActive(true);
        if ((currentMonsterNum >= limitMonsterNum && !GameManager.Instance.isGameOver) || Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.isGameOver = true;
            MessageManager.Instance.ShowMessage("GAME OVER", new Vector2(0, 218), 1f, 0.5f);
            StartCoroutine(CountdownCoroutine(5));
            isNormalSpawnStop = true;
        }
        else 
            waveSecTimer += Time.deltaTime;

        if (currentWaveMonsterNum == monsterSpawnNum[idx])
            isNormalSpawnStop = true;

        if (waveSecTimer >= 1f) 
        {
            waveTimer++;
            float remainingTime = wavePlayTime[idx] - waveTimer;
            remainingTime = Mathf.Max(0, remainingTime);
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
            UIManager.instance.currentWaveTime.text = timeSpan.ToString(@"mm\:ss");
            waveSecTimer = 0f;
        }
        if (waveTimer >= wavePlayTime[idx] && isAutoWaveProgression)
        {
            ProgressWave(1);
        }

        if (isSpawnStop) return;



        // 일반 몹 생성
        if (spawnDelayTimer >= monsterSpawnTime[idx] && !isNormalSpawnStop)
        {
            int monsterIndex = (GameManager.Instance.wave - 1) / 5;
            if (GameManager.Instance.wave % 5 == 0) monsterIndex -= 1;

            UnitCode code = (UnitCode)((int)UnitCode.MONSTER1 + monsterIndex);

            SpawnMonster(code);
        }
        // BOSS 생성
        if (isBossWave && !isBossSpawn)
        {
            UnitCode code = UnitCode.ELITEMONSTER5 + (GameManager.Instance.wave / 5);

            SpawnMonster(code);
            MessageManager.Instance.ShowMessage(targetBossStatus.unitCode.ToString() + "가 등장했습니다.", new Vector2(0, 200), 1f, 0.5f);

            isBossSpawn = true;
        }
        // WAVE SKIP
        if ((!isBossWave && isNormalSpawnStop) || (isBossWave && targetBossStatus.HP <= 0))
        {
            if (GameManager.Instance.isSKip)
            {
                ProgressWave(1);
            }
        }
    }
    
    public Monster SpawnMonster(UnitCode code)
    {
        var monster = MonsterPoolManager.Instance.GetPooledObject(code);
        if (monster == null)
        {
            Debug.LogError($"GetPooledObject returned null for code: {code}");
        }
        monster.transform.position = spawnPoints[spawnPointsCount - 1].transform.position;

        GameObject hpBar = MonsterHPBarPool.GetObject();
        hpBar.transform.parent = monster.transform;
        hpBar.transform.position = monster.transform.position + new Vector3(0,0.2f);
        monster.hpUI = hpBar;

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

    public void ProgressWave(int waveCnt)
    {
        if (GameManager.Instance.wave + waveCnt <= 0 || GameManager.Instance.wave + waveCnt > 36)
        {
            MessageManager.Instance.ShowMessage("유효하지 않은 스테이지입니다.", new Vector2(0, 200), 1f, 0.1f);
            return;
        }

        int limitMonsterNum = 80;

        if (GameManager.Instance.wave >= 11 && GameManager.Instance.wave <= 20)
            limitMonsterNum = 70;
        if (GameManager.Instance.wave >= 21 && GameManager.Instance.wave <= 30)
            limitMonsterNum = 60;
        if (GameManager.Instance.wave >= 31 && GameManager.Instance.wave <= 35)
            limitMonsterNum = 50;

        UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();

        if ((isBossWave && targetBossStatus.HP > 0) && !GameManager.Instance.isGameOver)
        {
            Restart();
        }

        GameManager.Instance.wave+= waveCnt;
        UIManager.instance.waveNum.text = "Wave " + GameManager.Instance.wave.ToString();

        if (waveCnt > 0)
            ElementManager.instance.GetElement(3);

        waveTimer = 0;
        isNormalSpawnStop = false;
        currentWaveMonsterNum = 0;
        isSpawnStop = false;
        isBossSpawn = false;
        isBossWave = bossSpawnNum[GameManager.Instance.wave - 1] >= 1;
        _skipButton.gameObject.SetActive(false);
    }

    public void WaveSkip()
    {
        if (currentWaveMonsterNum == monsterSpawnNum[GameManager.Instance.wave - 1] || isBossWave && targetBossStatus.HP <= 0)
        {
            ProgressWave(1);
        }
    }

    public void Restart()
    {
        GameManager.Instance.isGameOver = true;
        MessageManager.Instance.ShowMessage("GAME OVER", new Vector2(0, 218), 1f, 0.5f);
        StartCoroutine(CountdownCoroutine(5));
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        int countdown = seconds;
        while (countdown > 0)
        {
            
            // 메시지 매니저를 통해 카운트다운 메시지 표시
            MessageManager.Instance.ShowMessage($"Restart {countdown}", new Vector2(0, 200), 1f,0.1f);

            // 1초 기다리기
            yield return new WaitForSeconds(1f);

            // 카운트다운 감소
            countdown--;
        }

        // 5초 후에 씬 다시 로드
        ReloadScene();
    }

    private void ReloadScene()
    {
        // 현재 씬을 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


}
