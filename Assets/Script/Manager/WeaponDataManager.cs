using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponDataManager : Singleton<WeaponDataManager>
{
    public WeaponDatabase Database { get; private set; }

    protected override void Init()
    {
        Database = ResourceManager.Instance.Load<WeaponDatabase>("Database/WeaponDatabase");
        Database.Load();
        
        GameManager.Instance.weaponCnt = new int[Database.GetWeaponDataCount()];
    }
    
    public WeaponData GetWeaponData(int id)
    {
        return Database.GetWeaponData(id);
    }

    public string GetKorWeaponClassText(int weaponId)
    {
        string classEngText = Database.GetWeaponData(weaponId).WeaponClass;
        string classKorText = "";
        switch (classEngText)
        {
            case "unnormal":
                classKorText = "고급";
                break;
            case "rare":
                classKorText = "희귀";
                break;
            case "epic":
                classKorText = "영웅";
                break;
            case "legendary":
                classKorText = "전설";
                break;
            case "myth":
                classKorText = "신화";
                break;
            case "modified":
                classKorText = "변이";
                break;
        }

        return classKorText;
    }

    public string GetKorWeaponTypeText(int weaponId)
    {
        string typeKorText = "";
        int weaponType = Database.GetWeaponData(weaponId).Type1;

        switch (weaponType)
        {
            case 300 :
                typeKorText = "원소";
                break;
            case 301 :
                typeKorText = "단일";
                break;
            case 302 :
                typeKorText = "범위";
                break;
        }

        return typeKorText;
    }
    
    public string GetKorWeaponRTypeText(int weaponId)
    {
        string typeKorText = "";
        int weaponType = Database.GetWeaponData(weaponId).Type1;

        switch (weaponType)
        {
            case 100 :
                typeKorText = "빈칸";
                break;
            case 101 :
                typeKorText = "원거리";
                break;
            case 102 :
                typeKorText = "근거리";
                break;
            case 103 :
                typeKorText = "중거리";
                break;
            case 104 :
                typeKorText = "설치형";
                break;
        }

        return typeKorText;
    }
}
