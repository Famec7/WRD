using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponDataManager : MonoBehaviour
{
    // Start is called before the first frame update

    public WeaponData[] Data;
    public static WeaponDataManager instance;
    public WeaponBase[] weapons;
    
    private WeaponDatabase _weaponDatabase;
    private Dictionary<int, WeaponBase> _currentWeapons = new Dictionary<int, WeaponBase>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        List<Dictionary<string, object>> csvData = CSVReader.Read("Weapon");
        Data = new WeaponData[csvData.Count];
        
        for (int i = 0; i < csvData.Count; i++)
        {
            Data[i] = new WeaponData();
            Data[i].id = int.Parse(csvData[i]["id"].ToString());
            Data[i].weaponClass = (csvData[i]["class"].ToString());
            Data[i].weaponName = (csvData[i]["name"].ToString());
            //Data[i].type = int.Parse(csvData[i]["type"].ToString());
            Data[i].rType = int.Parse(csvData[i]["r_type"].ToString());
            Data[i].reload = int.Parse(csvData[i]["reload"].ToString());
            Data[i].reloadS = int.Parse(csvData[i]["reload_s"].ToString());
            Data[i].attackDamage = int.Parse(csvData[i]["attack"].ToString());
            Data[i].attackSpeed = float.Parse(csvData[i]["attack_speed"].ToString());
            Data[i].attackRange = float.Parse(csvData[i]["range"].ToString());
            // Data[i].attackAoe = float.Parse(csvData[i]["attack_AOE"].ToString());
            // Data[i].skillA = int.Parse(csvData[i]["skill_a"].ToString());
            //Data[i].skillP = int.Parse(csvData[i]["skill_p"].ToString());
            Data[i].combi = (csvData[i]["combi"].ToString());
            Data[i].mainCombi = (csvData[i]["comb1"].ToString());
            //Data[i].description = (csvData[i]["description"].ToString());
        }
    }

    private void Start()
    {
        _weaponDatabase = ResourceManager.Instance.Load<WeaponDatabase>("Database/WeaponDatabase");
    }

    public void SwitchWeapon(int id)
    {
        _currentWeapons[id].gameObject.SetActive(false);
        
        WeaponData weaponData = _weaponDatabase.GetWeaponData(id);

        foreach (var weapon in weapons)
        {
            if (weapon.Data == weaponData)
            {
                _currentWeapons[id] = weapon;
                break;
            }
        }
        
        if (_currentWeapons == null)
        {
            _currentWeapons[id] = gameObject.GetOrAddComponent<WeaponBase>();
            _currentWeapons[id].Data = weaponData;
            _currentWeapons[id].gameObject.SetActive(true);
        }
    }
    
    public WeaponData GetWeaponData(int id)
    {
        return _weaponDatabase.GetWeaponData(id);
    }
    
    public WeaponData GetWeaponData(string weaponName)
    {
        return _weaponDatabase.GetWeaponData(weaponName);
    }
}
