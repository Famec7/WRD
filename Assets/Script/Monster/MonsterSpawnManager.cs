using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


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
    bool isWait = false;

    [SerializeField]
    private Canvas _hpBarCanvas;

    public Button _skipButton;
    public bool isAutoWaveProgression = false;


    [SerializeField]
    private Image _tutorialImage;

    [SerializeField]
    private Sprite[] _tutorialSprites; // 10개 이미지


    [SerializeField]
    private GameObject[] _uis;

    private int _tutorialIndex = 0;
    private bool _isTutorialMode = true;
    private Vector2 _mouseDownPos;
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



        UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();
        TimeSpan timeSpan = TimeSpan.FromSeconds(wavePlayTime[0]);
        UIManager.instance.currentWaveTime.text = timeSpan.ToString(@"mm\:ss");

        UIManager.instance.waveNum.text = "Wave " + GameManager.Instance.wave.ToString();

        StartTutorial();
    }

    private void StartTutorial()
    {
        _isTutorialMode = true;
        _tutorialIndex = 0;
        Time.timeScale = 0f;

        _tutorialImage.gameObject.SetActive(true);
        for (int i = 0; i < _uis.Length; i++)
            _uis[i].gameObject.SetActive(false);
        _tutorialImage.sprite = _tutorialSprites[_tutorialIndex];
    }

    private void SkipTutorial()
    {
        _isTutorialMode = false;
        _tutorialImage.gameObject.SetActive(false);
        for (int i = 0; i < _uis.Length; i++)
            _uis[i].gameObject.SetActive(true);
        StartCoroutine(CountdownStartCoroutine(3));
    }
    // Update is called once per frame
    void Update()
    {
        if (_isTutorialMode)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // 마우스 클릭 처리 (PC)
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPos = Input.mousePosition;
                float screenHalf = Screen.width / 2f;

                if (clickPos.x < screenHalf)
                {
                    // 이전 이미지
                    _tutorialIndex = Mathf.Max(0, _tutorialIndex - 1);
                }
                else
                {
                    // 다음 이미지
                    _tutorialIndex++;
                }

                if (_tutorialIndex < _tutorialSprites.Length)
                {
                    _tutorialImage.sprite = _tutorialSprites[_tutorialIndex];
                }
                else
                {
                    SkipTutorial();
                }
            }

#if UNITY_EDITOR
            // 마우스로 스와이프 테스트
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 upPos = Input.mousePosition;
                float deltaY = upPos.y - _mouseDownPos.y;

                if (deltaY < -50f) // 아래로 드래그
                {
                    SkipTutorial();
                }
            }
#else
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Moved)
        {
            if (touch.deltaPosition.y < -50f)
            {
                SkipTutorial();
            }
        }
    }
#endif

            return;
        }


        UIManager.instance.currentMonsterNum.text = currentMonsterNum.ToString();
        if (!isWait) return;


        if (GameManager.Instance.wave >= 11 && GameManager.Instance.wave <= 20)
            limitMonsterNum = 70;
        if (GameManager.Instance.wave >= 21 && GameManager.Instance.wave <= 30)
            limitMonsterNum = 60;
        if (GameManager.Instance.wave >= 31 && GameManager.Instance.wave <= 35)
            limitMonsterNum = 50;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1.0f;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2.0f;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 3.0f;

        if (GameManager.Instance.isGameOver || GameManager.Instance.isGameClear) return;

        int idx = GameManager.Instance.wave - 1;
        isBossWave = bossSpawnNum[idx] >= 1;

        if (idx >= 34)
            idx = 34;

        spawnDelayTimer += Time.deltaTime;
        if (currentMonsterNum >= currentWaveMonsterNum)
            _skipButton.gameObject.SetActive(true);
        if ((currentMonsterNum >= limitMonsterNum && !GameManager.Instance.isGameOver) || Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.isGameOver = true;
            UIManager.instance.ResultUI.gameObject.SetActive(true);
            UIManager.instance.ResultUI.SetResultUI(false);
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
        if (waveTimer >= wavePlayTime[idx] && isAutoWaveProgression && !isBossWave)
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
            if ((int)UnitCode.MONSTER1 + monsterIndex != -1)
                SpawnMonster(code);
        }
        // BOSS 생성
        if (isBossWave && !isBossSpawn)
        {
            UnitCode code = UnitCode.ELITEMONSTER5 + (GameManager.Instance.wave / 5);
            if (code == UnitCode.MISSIONBOSS1)
                code = UnitCode.BOSS6;
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
            else if (waveTimer >= wavePlayTime[idx])
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
        hpBar.transform.position = monster.transform.position + new Vector3(0, 0.2f);
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
            GameManager.Instance.isGameOver = true;
            UIManager.instance.ResultUI.gameObject.SetActive(true);
            UIManager.instance.ResultUI.SetResultUI(false);
            isNormalSpawnStop = true;
        }

        if (GameManager.Instance.wave == 35)
        {
            isNormalSpawnStop = true;
            UIManager.instance.ResultUI.gameObject.SetActive(true);
            UIManager.instance.ResultUI.SetResultUI(true);
            GameManager.Instance.isGameClear = true;
            return;

        }

        GameManager.Instance.wave += waveCnt;
        UIManager.instance.waveNum.text = "Wave " + GameManager.Instance.wave.ToString();

        if (waveCnt > 0 && GameManager.Instance.wave <= 30)
            ElementManager.instance.GetElement(3);

        waveTimer = 0;
        isNormalSpawnStop = false;
        currentWaveMonsterNum = 0;
        isSpawnStop = false;
        isBossSpawn = false;
        isBossWave = bossSpawnNum[GameManager.Instance.wave - 1] >= 1;
        if (isBossWave && GameManager.Instance.wave <= 30)
            isNormalSpawnStop = true;
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
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        int countdown = seconds;
        while (countdown > 0)
        {

            // 메시지 매니저를 통해 카운트다운 메시지 표시
            MessageManager.Instance.ShowMessage($"Restart {countdown}", new Vector2(0, 200), 1f, 0.1f);

            // 1초 기다리기
            yield return new WaitForSeconds(1f);

            // 카운트다운 감소
            countdown--;
        }

        // 5초 후에 씬 다시 로드
        ReloadScene();
    }

    private IEnumerator CountdownStartCoroutine(int seconds)
    {
        int countdown = seconds;
        UIManager.instance.GameStartImage.gameObject.SetActive(true);
        Time.timeScale = 1f;
        while (countdown > 0)
        {
            UIManager.instance.GameStartImage.sprite = UIManager.instance.GameStartSprites[countdown - 1];
            // 1초 기다리기
            yield return new WaitForSeconds(1f);

            // 카운트다운 감소
            countdown--;
        }
        StartCoroutine(DisableStartImage());
        isWait = true;
    }

    public void ReloadScene()
    {
        // 현재 씬을 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    IEnumerator DisableStartImage()
    {
        UIManager.instance.GameStartImage.sprite = UIManager.instance.GameStartSprites[3];
        yield return new WaitForSeconds(1f);
        UIManager.instance.GameStartImage.gameObject.SetActive(false);
    }
}
