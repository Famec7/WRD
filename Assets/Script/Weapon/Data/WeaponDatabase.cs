using System;
using System.Collections.Generic;
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
        
        foreach (var data in csvData)
        {
            WeaponData weaponData = new WeaponData
            {
                id = int.Parse(data["id"].ToString()),
                weaponClass = (data["class"].ToString()),
                weaponName = (data["name"].ToString()),
                rType = int.Parse(data["r_type"].ToString()),
                reload = int.Parse(data["reload"].ToString()),
                reloadS = int.Parse(data["reload_s"].ToString()),
                attackDamage = int.Parse(data["attack"].ToString()),
                attackSpeed = float.Parse(data["attack_speed"].ToString()),
                attackRange = float.Parse(data["range"].ToString()),
                combi = (data["combi"].ToString()),
                mainCombi = (data["comb1"].ToString())
            };

            _weaponDataList.Add(weaponData);
        }
    }
    
    public WeaponData GetWeaponData(int id)
    {
        foreach (var data in _weaponDataList)
        {
            if (data.id == id)
            {
                return data;
            }
        }

        return null;
    }
    
    public WeaponData GetWeaponData(string weaponName)
    {
        foreach (var data in _weaponDataList)
        {
            // 대소문자, 공백 없이 비교
            if (string.Compare(data.weaponName.Replace(" ",""), weaponName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return data;
            }
        }

        Debug.LogError($"WeaponData {weaponName} is not found");
        return null;
    }
}