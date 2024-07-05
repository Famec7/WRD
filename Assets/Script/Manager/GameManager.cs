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
    public int[] useAbleWeaponCnt;

    public List<int> useWeapon;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        useWeapon = new List<int>();
    }

    void Start()
    {
        weaponCnt = new int[WeaponDataManager.Instance.Database.GetWeaponDataCount()];
        GameManager.instance.weaponCnt[6]++;
        useWeapon.Add(7);
        UpdateUseableWeaponCnt();
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
                useAbleWeaponCnt[id - 1]++;
                useWeapon.Remove(id);
                break;
            }
        }
    }

    public void UpdateUseableWeaponCnt()
    {
        useAbleWeaponCnt = (int[])weaponCnt.Clone();

        foreach (var id in useWeapon)
        {
            useAbleWeaponCnt[id - 1]--;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
