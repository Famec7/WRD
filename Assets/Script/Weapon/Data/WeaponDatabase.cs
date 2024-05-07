using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Scriptable Object/WeaponDatabase", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> weaponDataList;

    [ContextMenu("Load")]
    public void Load()
    {
        List<Dictionary<string, object>> csvData = CSVReader.Read("Weapon");
        weaponDataList = new List<WeaponData>(csvData.Count);
        
        for (int i = 0; i < csvData.Count; i++)
        {
            WeaponData weaponData = new WeaponData();
            weaponData.id = int.Parse(csvData[i]["id"].ToString());
            weaponData.weaponClass = (csvData[i]["class"].ToString());
            weaponData.weaponName = (csvData[i]["name"].ToString());
            //Data[i].type = int.Parse(csvData[i]["type"].ToString());
            weaponData.rType = int.Parse(csvData[i]["r_type"].ToString());
            weaponData.reload = int.Parse(csvData[i]["reload"].ToString());
            weaponData.reloadS = int.Parse(csvData[i]["reload_s"].ToString());
            weaponData.attackDamage = int.Parse(csvData[i]["attack"].ToString());
            weaponData.attackSpeed = float.Parse(csvData[i]["attack_speed"].ToString());
            weaponData.attackRange = float.Parse(csvData[i]["range"].ToString());
            // Data[i].attackAoe = float.Parse(csvData[i]["attack_AOE"].ToString());
            // Data[i].skillA = int.Parse(csvData[i]["skill_a"].ToString());
            //Data[i].skillP = int.Parse(csvData[i]["skill_p"].ToString());
            weaponData.combi = (csvData[i]["combi"].ToString());
            weaponData.mainCombi = (csvData[i]["comb1"].ToString());
            //Data[i].description = (csvData[i]["description"].ToString());
        }
    }
    
    public WeaponData GetWeaponData(int id)
    {
        foreach (var data in weaponDataList)
        {
            if (data.id == id)
            {
                return data;
            }
        }

        return null;
    }
}