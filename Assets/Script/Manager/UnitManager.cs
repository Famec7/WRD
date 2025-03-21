using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static UnitManager instance;
    public List<GameObject> monsterList = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveAllMonster()
    {
        foreach(var m in monsterList)
        {
          m.GetComponent<Monster>().HasAttacked(m.GetComponent<Status>().HP);
        }
    }    
    
}
