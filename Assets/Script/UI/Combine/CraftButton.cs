using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isMain = false;
    public int weaponID;
    public int[] materialWeapons;
    public bool isElement = false;

    public GameObject canCombineBorder;

    void Start()
    {
        isElement = weaponID < 6 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.weaponCnt[weaponID-1] > 0 && !isMain)
        {
            gameObject.GetComponent<Button>().enabled = false;
            canCombineBorder.SetActive(false);
            return;
        }

        bool isMasterKey = false;

        if (isElement)
        {
            gameObject.GetComponent<Button>().enabled = false;
            if (MasterKeyManager.Instance.masterKeyCnt[weaponID - 1] > 0 && !isMain)
            {
                canCombineBorder.GetComponent<Image>().color = new Color32(255, 71, 40, 255);
            }
        }

        Color32 mainColor = GetClassColor(WeaponDataManager.Instance.Database.GetWeaponData(weaponID).WeaponClass);
        
        if (GameManager.Instance.weaponCnt[weaponID - 1] > 0)
            transform.GetComponent<Image>().color = mainColor;
        else
            transform.GetComponent<Image>().color = new Color32(56, 56, 56, 255); ;

        int hasMaterialCnt = 0;
        int[] tmpCnt = new int[GameManager.Instance.weaponCnt.Length];
        int[] tmpMasterKeyCnt = new int[MasterKeyManager.Instance.masterKeyCnt.Length];

        List<int> absentWeaponList = new List<int>();

        Array.Copy(GameManager.Instance.weaponCnt, tmpCnt, tmpCnt.Length);
        Array.Copy(MasterKeyManager.Instance.masterKeyCnt, tmpMasterKeyCnt, tmpMasterKeyCnt.Length);

        foreach (int i in materialWeapons)
        {
            if (tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1]--;
            }
            else
            {
                absentWeaponList.Add(i);
                isMasterKey = true;
            }
        }

        List<WeaponData> absentWeaponData = new List<WeaponData>();

        foreach (int id in absentWeaponList)
            absentWeaponData.Add(WeaponDataManager.Instance.Database.GetWeaponDataByNum(id));

        foreach (var data in absentWeaponData)
        {
            if (tmpMasterKeyCnt[(int)data.tier - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpMasterKeyCnt[(int)data.tier - 1]--;
            }
        }

        WeaponData weaponData = WeaponDataManager.Instance.GetWeaponData(weaponID);

        if (hasMaterialCnt == materialWeapons.Length && !isMasterKey)
        {
            canCombineBorder.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            canCombineBorder.SetActive(true);
        }
        else if (MasterKeyManager.Instance.masterKeyCnt[(int)weaponData.tier - 1] > 0  && !isElement)
        {
            canCombineBorder.GetComponent<Image>().color = new Color32(255, 71, 40, 255);
            canCombineBorder.SetActive(true);
        }
        else
            canCombineBorder.SetActive(false);
            
    }

    public void CraftWeapon(bool isMainWeapon = false)
    {
        int hasMaterialCnt = 0;
        int[] tmpCnt = new int[GameManager.Instance.weaponCnt.Length];
        int[] tmpMasterKeyCnt = new int[MasterKeyManager.Instance.masterKeyCnt.Length];

        List<int>absentWeaponList = new List<int>();
        Array.Copy(GameManager.Instance.weaponCnt, tmpCnt, tmpCnt.Length);
        Array.Copy(MasterKeyManager.Instance.masterKeyCnt, tmpMasterKeyCnt, tmpMasterKeyCnt.Length);
        WeaponData weaponData = WeaponDataManager.Instance.GetWeaponData(weaponID);

        foreach (int i in materialWeapons)
        {
            if (tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1]--;
            }
            else
                absentWeaponList.Add(i);
        }

        if (hasMaterialCnt == materialWeapons.Length)
        {
            List<int> materialIDList = new List<int>();
            foreach (int i in materialWeapons)
            {
                materialIDList.Add(WeaponDataManager.Instance.Database.GetWeaponIdByNum(i));
            }
            InventoryItem item = new InventoryItem
            {
                image = transform.GetChild(0).GetComponent<Image>().sprite
            };
            item.AssignWeapon(weaponID);
            WeaponUI.Instance.weaponID = weaponID;

            InventoryManager.instance.AddItem(item, false);
            InventoryManager.instance.RemoveItem(materialIDList, weaponID, item, isMainWeapon);

            if (InventoryManager.instance.isClassSorted)
                InventoryManager.instance.ClickClassShowButton();

            if(isMainWeapon)
                UIManager.instance.CreateCombineUI(weaponID);

            GameManager.Instance.weaponCnt[weaponID - 1]++;
            GameManager.Instance.UpdateUseableWeaponCnt();
            BookMakredSlotUI.Instance.UpdateAllSlot();

        }
        else 
        {
           List<WeaponData> absentWeaponData = new List<WeaponData>();

           foreach(int id in absentWeaponList)
            absentWeaponData.Add(WeaponDataManager.Instance.Database.GetWeaponDataByNum(id));

           List<int> materialIDList = new List<int>();
           foreach (int i in materialWeapons)
           {
               materialIDList.Add(WeaponDataManager.Instance.Database.GetWeaponIdByNum(i));
           }

           foreach (var data in absentWeaponData)
           {
                if (tmpMasterKeyCnt[(int)data.tier - 1] >= 1)
                {
                    hasMaterialCnt++;
                    tmpMasterKeyCnt[(int)data.tier - 1]--;
                    materialIDList.Remove(data.ID);
                }
           }

           if (hasMaterialCnt == materialWeapons.Length)
            {
                InventoryManager.instance.OpenWeaponPickerConfirmPopUp(absentWeaponList,weaponID, materialIDList , isMain);
            }
        }
    }

    public Color32 GetClassColor(string classStr)
    {
        Color32 color = new Color32(56, 56, 56, 255);

        switch (classStr)
        {
            case "element":
                color = new Color32(0, 0, 0, 255);
                break;
            case "unnormal":
                color = new Color32(84, 130, 53, 255);
                break;
            case "rare":
                color = new Color32(68, 114, 196, 255);
                break;
            case "epic":
                color = new Color32(112, 48, 160, 255);
                break;
            case "legendary":
                color = new Color32(255, 192, 0, 255);
                break;
            case "myth":
                color = new Color32(255, 255, 255, 255);
                break;
        }

        return color;
    }
}
