using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int wave = 1;
    public bool isSKip = false;
    public bool isGameOver = false;
    public int[] weaponCnt;
    public int[] useAbleWeaponCnt;
    public int StartElementCnt = 5;
    public List<int> useWeapon;

    protected override void Init()
    {
        useWeapon = new List<int>();
    }

    private void Start()
    {
        weaponCnt = new int[WeaponDataManager.Instance.Database.GetWeaponDataCount()];
        GameManager.Instance.weaponCnt[5]++;
        useWeapon.Add(6);
        UpdateUseableWeaponCnt();
        ElementManager.instance.GetElement(StartElementCnt);
    }

    public bool IsUsing(int weaponID)
    {
        foreach (var usingWeapon in useWeapon)
        {
            if (usingWeapon == weaponID)
            {
                return true;
            }
        }
        
        return false;
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
}
