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

    public List<int> useWeapon;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        useWeapon = new List<int>();
    }

    void Start()
    {
        weaponCnt = new int[WeaponDataManager.instance.Data.Length];
        GameManager.instance.weaponCnt[6]++;
        useWeapon.Add(7);
        ElementManager.instance.GetElement(100);
    }

    public bool isUsing(int _weaponID)
    {
        bool result = false;
        
        foreach (var weaponID in useWeapon)
        {
            if (weaponID == _weaponID)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public void RemoveUseWeaponList(int weaponID)
    {
        foreach (var id in useWeapon)
        {
            if (id == weaponID)
            {
                useWeapon.Remove(id);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
