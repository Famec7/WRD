using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : Singleton<MonsterPoolManager>
{
    // Start is called before the first frame update

    /*public GameObject[] monsterPrefab;
    
    private Dictionary<UnitCode, List<GameObject>> pooledObjects = new Dictionary<UnitCode, List<GameObject>>();
    int poolingCount = 100;*/
    
    private PoolManager _poolManager;

    protected override void Init()
    {
        _poolManager = GetComponent<PoolManager>();
    }

    /*public void CreateMultiplePoolObjects()
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
    }*/

    public Monster GetPooledObject(UnitCode code)
    {
        var pooledObject = _poolManager.GetFromPool<Monster>(code.ToString());

        if (pooledObject is null)
        {
            Debug.LogError("${code} is not found in the pool.");
            return null;
        }
        
        if(code >= UnitCode.MISSIONBOSS1)
            pooledObject.GetComponent<Status>().SetMissionUnitStatus(code);
        else
            pooledObject.GetComponent<Status>().SetUnitStatus(code);

        if (code >= UnitCode.BOSS1)
        {
            MonsterSpawnManager.instance.targetBossStatus = pooledObject.GetComponent<Status>();
            MonsterSpawnManager.instance.targetBoss = pooledObject;

        }
        
        pooledObject.GetComponent<Basic_Monster>().isDead = false;
        pooledObject.GetComponent<Monster>().isDead = false;
        pooledObject.transform.SetParent(null);

        return pooledObject;
    }

    public void ReturnObject(GameObject obj)
    {
        _poolManager.ReturnToPool(obj.GetComponent<Monster>());
        MonsterSpawnManager.instance.currentMonsterNum--;
    }

    public void ReturnObject(string name, GameObject obj)
    {
        _poolManager.ReturnToPool(name, obj.GetComponent<Monster>());
        MonsterSpawnManager.instance.currentMonsterNum--;
    }
}
