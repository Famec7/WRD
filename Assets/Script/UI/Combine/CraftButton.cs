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
        if (GameManager.Instance.weaponCnt[weaponID-1] > 0 && !isMain || isElement)
        {
            gameObject.GetComponent<Button>().enabled = false;
            canCombineBorder.SetActive(false);
            return;
        }


        Color32 mainColor = GetClassColor(WeaponDataManager.Instance.Database.GetWeaponData(weaponID).WeaponClass);

        if (GameManager.Instance.weaponCnt[weaponID - 1] > 0)
            transform.GetComponent<Image>().color = mainColor;
        else
            transform.GetComponent<Image>().color = new Color32(56, 56, 56, 255); ;

        int hasMaterialCnt = 0;
        int[] tmpCnt = new int[GameManager.Instance.weaponCnt.Length];
        Array.Copy(GameManager.Instance.weaponCnt, tmpCnt, tmpCnt.Length);

        foreach (int i in materialWeapons)
        {
            if (tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1]--;
            }
        }

 

        if (hasMaterialCnt == materialWeapons.Length)
            canCombineBorder.SetActive(true);
        else
            canCombineBorder.SetActive(false);
    }

    public void CraftWeapon(bool isMainWeapon = false)
    {
        int hasMaterialCnt = 0;
        int[] tmpCnt = new int[GameManager.Instance.weaponCnt.Length];
        Array.Copy(GameManager.Instance.weaponCnt, tmpCnt, tmpCnt.Length);

        foreach (int i in materialWeapons)
        {
            if (tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpCnt[WeaponDataManager.Instance.Database.GetWeaponIdByNum(i) - 1]--;
            }
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
