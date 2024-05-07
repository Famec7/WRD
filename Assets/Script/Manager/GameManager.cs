using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager instance;

    public int wave = 1;
    public bool isSKip = false;
    public int[] weaponCnt;

    

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        weaponCnt = new int[WeaponDataManager.instance.Data.Length];
        GameManager.instance.weaponCnt[6]++;
        ElementManager.instance.GetElement(100);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
