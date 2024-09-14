using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : Singleton<MonsterPoolManager>
{
    // Start is called before the first frame update

    public GameObject[] monsterPrefab;
    
    private Dictionary<UnitCode, List<GameObject>> pooledObjects = new Dictionary<UnitCode, List<GameObject>>();
    int poolingCount = 100;
    
    private PoolManager _poolManager;

    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
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

    public Monster GetPooledObject(UnitCode code)
    {
        /*if (pooledObjects.ContainsKey(code))
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
        }*/

        var pooledObject = _poolManager.GetFromPool<Monster>(code.ToString());

        if (pooledObjects is null)
        {
            Debug.LogError("${code} is not found in the pool.");
            return null;
        }
        
        if(code >= UnitCode.MISSIONBOSS1)
            pooledObject.GetComponent<Status>().SetMissionUnitStatus(code);
        else
            pooledObject.GetComponent<Status>().SetUnitStatus(code);

        if ((int)code > 5)
        {
            MonsterSpawnManager.instance.targetBossStatus = pooledObject.GetComponent<Status>();
        }
        
        pooledObject.GetComponent<Basic_Monster>().isDead = false;
        pooledObject.transform.SetParent(null);

        return pooledObject;
    }

    public void ReturnObject(GameObject obj)
    {
        /*obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        pooledObjects[obj.GetComponent<Status>().unitCode].Add(obj);*/
        
        _poolManager.ReturnToPool(obj.GetComponent<Monster>());
        MonsterSpawnManager.instance.currentMonsterNum--;
    }
}
