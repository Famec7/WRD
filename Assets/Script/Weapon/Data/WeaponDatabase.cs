using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Object/WeaponDatabase", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    private List<WeaponData> _weaponDataList;

    [ContextMenu("Load")]
    public void Load()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("Weapon");
        _weaponDataList = new List<WeaponData>(csvData.Count);
        int idx = 1;
        foreach (var data in csvData)
        {
            WeaponData weaponData = new WeaponData
            {
                ID = idx++,
                WeaponClass = (data["class"].ToString()),
                WeaponName = (data["name"].ToString()),
                rType = int.Parse(data["r_type"].ToString()),
                num = int.Parse(data["num"].ToString()),
                //reload = int.Parse(data["reload"].ToString()),
                //reloadS = int.Parse(data["reload_s"].ToString()),
                AttackDamage = int.Parse(data["attack"].ToString()),
                AttackSpeed = float.Parse(data["attack_speed"].ToString()),
                AttackRange = float.Parse(data["range"].ToString()),
                Combi = (data["combi"].ToString()),
                MainCombi = (data["comb1"].ToString())
            };

            switch (weaponData.WeaponClass.ToLower()) // 대소문자 구분 없이 비교
            {
                case "normal":
                    weaponData.tier = WeaponTier.Element ;
                    break;
                case "unnormal":
                    weaponData.tier = WeaponTier.UNNORMAL;
                    break;
                case "rare":
                    weaponData.tier = WeaponTier.RARE;
                    break;
                case "epic":
                    weaponData.tier = WeaponTier.EPIC;
                    break;
                case "legendary":
                    weaponData.tier = WeaponTier.LEGENDARY;
                    break;
                default:
                    weaponData.tier = WeaponTier.Empty; // 기본값 설정
                    Debug.LogWarning($"Unrecognized WeaponClass: {weaponData.WeaponClass}");
                    break;
            }
            _weaponDataList.Add(weaponData);
        }
       
    }
    
    public WeaponData GetWeaponData(int id)
    {
        foreach (var data in _weaponDataList)
        {
            if (data.ID == id)
            {
                return data;
            }
        }

        return null;
    }
    
    public int GetWeaponIdByNum(int num)
    {
        foreach (var data in _weaponDataList)
        {
            if (data.num == num)
                return data.ID;
        }

        return 0;
    }

    public int GetWeaponNumByID(int id)
    {
        foreach (var data in _weaponDataList)
        {
            if (data.ID == id)
                return data.num;
        }

        return 0;
    }

    public int GetWeaponDataCount()
    {
        return _weaponDataList.Count;
    }

    public List<int> GetAllWeaponNums()
    {
        return _weaponDataList.Select(data => data.num).ToList();
    }

    public WeaponData GetWeaponDataByNum(int num)
    {
        foreach(var data in _weaponDataList)
        {
            if (data.num == num)
                return data;
        }

        return null;
    }

    public List<WeaponData> GetAllSameTierWeaponData(WeaponTier tier)
    {
        List<WeaponData> datas = new List<WeaponData>();
        
        foreach (var data in _weaponDataList)
        {

            if (data.tier == tier)
            {
                Debug.Log(tier);
                datas.Add(data);
            }
        }

        return datas;
    }
}
