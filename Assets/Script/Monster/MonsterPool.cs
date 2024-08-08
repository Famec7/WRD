using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] monsterPrefab;

    public static MonsterPool instance;
    private Dictionary<UnitCode, List<GameObject>> pooledObjects = new Dictionary<UnitCode, List<GameObject>>();
    int poolingCount = 100;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        CreateMultiplePoolObjects();

    }

    public void CreateMultiplePoolObjects()
    {
        for (int i = 0; i < monsterPrefab.Length; i++)
        {
            if (i > 5) 
                poolingCount = 1;

            for (int j = 0; j < poolingCount; j++)
            {
                if (!pooledObjects.ContainsKey(monsterPrefab[i].GetComponent<Status>().unitCode))
                {
                    List<GameObject> newList = new List<GameObject>();
                    pooledObjects.Add(monsterPrefab[i].GetComponent<Status>().unitCode, newList);
                }

                GameObject enemy = Instantiate(monsterPrefab[i], transform);
                enemy.SetActive(false);
                pooledObjects[monsterPrefab[i].GetComponent<Status>().unitCode].Add(enemy);
            }
        }
    }

    public GameObject GetPooledObject(UnitCode code)
    {
        if (pooledObjects.ContainsKey(code))
        {
            for (int i = 0; i < pooledObjects[code].Count; i++)
            {
                if (!pooledObjects[code][i].activeSelf)
                {
                    if (code >= UnitCode.MISSIONBOSS1)
                        pooledObjects[code][i].GetComponent<Status>().SetMissionUnitStatus(code);
                    else
                        pooledObjects[code][i].GetComponent<Status>().SetUnitStatus(code);

                    if ((int)code > 5)                    
                        MonsterSpawnManager.instance.targetBossStatus = pooledObjects[code][i].GetComponent<Status>();
                    

                    pooledObjects[code][i].GetComponent<Basic_Monster>().isDead = false;
                    pooledObjects[code][i].transform.SetParent(null);
                    pooledObjects[code][i].gameObject.SetActive(true);

                    return pooledObjects[code][i];
                }
            }

            int beforeCreateCount = pooledObjects[code].Count;

            CreateMultiplePoolObjects();

            if (code >= UnitCode.MISSIONBOSS1)
                pooledObjects[code][beforeCreateCount].GetComponent<Status>().SetMissionUnitStatus(code);
            else
                pooledObjects[code][beforeCreateCount].GetComponent<Status>().SetUnitStatus(code);

            pooledObjects[code][beforeCreateCount].GetComponent<Basic_Monster>().isDead = false;
            pooledObjects[code][beforeCreateCount].transform.SetParent(null);
            pooledObjects[code][beforeCreateCount].gameObject.SetActive(true);


            return pooledObjects[code][beforeCreateCount];
        }

        else
        {
            return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        pooledObjects[obj.GetComponent<Status>().unitCode].Add(obj);
        MonsterSpawnManager.instance.currentMonsterNum--;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
