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
    public bool isSpawnStop = false;

    public float spawnDelayTimer = 0f;
    public float maxSpawnDelay = 2f;

    float waveTimer = 0f;
    float waveSecTimer = 0f;

    public GameObject[] spawnPoints;
    public GameObject hpBarPrefab;

    public int spawnPointsCount = 1;
    public int currentMonsterNum = 0;
    public int currentWaveMonsterNum = 0;
    public int limitMonsterNum = 80;

    int[] waveNum;
    int[] monsterSpawnNum;
    int[] bossSpawnNum;
    int[] wavePlayTime;

    float[] monsterSpawnTime;

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
            waveNum[i] = int.Parse(data[i]["waveNum"].ToString());
            monsterSpawnNum[i] = int.Parse(data[i]["monsterSpawnNum"].ToString());
            bossSpawnNum[i] = int.Parse(data[i]["bossSpawnNum"].ToString());
            wavePlayTime[i] = int.Parse(data[i]["wavePlaytime"].ToString());
            monsterSpawnTime[i] = float.Parse((data[i]["monsterSpawnTime"]).ToString());
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

        if (idx >= 34)
            idx = 34;

        spawnDelayTimer += Time.deltaTime;

        if (currentMonsterNum >= limitMonsterNum)
            isSpawnStop = true;
        else
            waveSecTimer += Time.deltaTime;

        if (waveSecTimer >= 1f)
        {
            waveTimer++;
            UIManager.instance.currentWaveTime.text = (wavePlayTime[idx] - waveTimer).ToString();
            waveSecTimer = 0f;
        }

        if (waveTimer >= wavePlayTime[idx])
        {
            GameManager.instance.wave++;
            UIManager.instance.waveNum.text = GameManager.instance.wave.ToString();

            currentWaveMonsterNum = 0;

            int limitMonsterNum = 80;

            if (GameManager.instance.wave >= 11 && GameManager.instance.wave <= 20)
                limitMonsterNum = 70;
            if (GameManager.instance.wave >= 21 && GameManager.instance.wave <= 30)
                limitMonsterNum = 60;
            if (GameManager.instance.wave >= 31 && GameManager.instance.wave <= 35)
                limitMonsterNum = 50;

            UIManager.instance.limitMonsterNum.text = limitMonsterNum.ToString();

            ElementManager.instance.GetElement(3);
            waveTimer = 0;

            isSpawnStop = false;
        }

        if (isSpawnStop) return;

        if (spawnDelayTimer >= 0.5f)
        {
            UnitCode code = (UnitCode)((int)GameManager.instance.wave / 5);

            GameObject monster = MonsterPool.instance.GetPooledObject(code);
            monster.GetComponent<MonsterMoveComponent>().roadNum = 1; // *임시코드* 생성 위치 바꾸면 코드 수정해야함

            GameObject hpBar = Instantiate(hpBarPrefab, GameObject.Find("Canvas").transform);
            hpBar.GetComponent<MonsterHPBar>().owner = monster;
            hpBar.GetComponent<MonsterHPBar>().ownerStatus = monster.GetComponent<Status>();

            hpBar.transform.SetAsFirstSibling();

            monster.transform.position = spawnPoints[spawnPointsCount - 1].transform.position;
            hpBar.transform.position = Camera.main.WorldToScreenPoint(new Vector3(monster.transform.position.x, monster.transform.position.y + 0.1f, 0));
            //Camera mainCamera = Camera.main;

            //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, monster.transform.position + new Vector3(0.01f, 0.1f, 0));

            //RectTransform canvasRectTransForm = GameObject.Find("Canvas").GetComponent<RectTransform>();
            //Vector2 localPoint;

            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransForm, screenPoint, mainCamera, out localPoint))
            //{
            //    hpBar.GetComponent<RectTransform>().localPosition = localPoint;

            //}

            spawnDelayTimer = 0;
            currentMonsterNum++;
            currentWaveMonsterNum++;

            UIManager.instance.currentMonsterNum.text = currentMonsterNum.ToString();
            UnitManager.instance.monsterList.Add(monster);

            if (currentWaveMonsterNum == monsterSpawnNum[idx])
            {
                isSpawnStop = true;

                if (GameManager.instance.isSKip)
                {
                    waveTimer = wavePlayTime[idx];
                    isSpawnStop = false;
                }
            }
        }
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
