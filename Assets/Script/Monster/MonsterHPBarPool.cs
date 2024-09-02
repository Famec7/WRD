using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;

public class MonsterHPBarPool : MonoBehaviour
{
    [SerializeField]
    private GameObject poolingObjectPrefab;
    public static MonsterHPBarPool Instance;
    // Start is called before the first frame update
    Queue<MonsterHPBar> hpBarQueue = new Queue<MonsterHPBar>();
    private void Awake()
    {
        Instance = this;

        Initialize(30);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            hpBarQueue.Enqueue(CreateNewObject());
        }
    }

    private MonsterHPBar CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<MonsterHPBar>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static void ReturnObject(MonsterHPBar obj)
    {
        obj.gameObject.SetActive(false);
        obj.Init();
        obj.transform.SetParent(Instance.transform);
        Instance.hpBarQueue.Enqueue(obj);
    }

    public static GameObject GetObject()
    {
        if (Instance.hpBarQueue.Count > 0)
        {
            var obj = Instance.hpBarQueue.Dequeue();
            return obj.gameObject;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj.gameObject;
        }
    }
}
