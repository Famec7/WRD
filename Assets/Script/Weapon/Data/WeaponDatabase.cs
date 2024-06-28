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
        
        for (int i = 0; i < csvData.Count; i++)
        {
            WeaponData weaponData = new WeaponData();
            weaponData.id = int.Parse(csvData[i]["id"].ToString());
            weaponData.weaponClass = (csvData[i]["class"].ToString());
            weaponData.weaponName = (csvData[i]["name"].ToString());
            weaponData.rType = int.Parse(csvData[i]["r_type"].ToString());
            weaponData.reload = int.Parse(csvData[i]["reload"].ToString());
            weaponData.reloadS = int.Parse(csvData[i]["reload_s"].ToString());
            weaponData.attackDamage = int.Parse(csvData[i]["attack"].ToString());
            weaponData.attackSpeed = float.Parse(csvData[i]["attack_speed"].ToString());
            weaponData.attackRange = float.Parse(csvData[i]["range"].ToString());
            weaponData.combi = (csvData[i]["combi"].ToString());
            weaponData.mainCombi = (csvData[i]["comb1"].ToString());
            
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
            if (string.Compare(data.weaponName.Replace(" ", "").ToLower(), weaponName.Replace(" ", "").ToLower(), StringComparison.Ordinal) == 0)
            {
                return data;
            }
        }

        Debug.LogError($"WeaponData {weaponName} is not found");
        return null;
    }
}