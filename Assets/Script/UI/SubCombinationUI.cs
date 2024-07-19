using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;


public class SubCombinationUI : MonoBehaviour
{
    // Start is called before the first frame update
    public int mainweaponID;
    public Vector3 originPosition;
    public int[] materialWeapons;
    public GameObject canCombineBorder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int i = 2;
        int cnt = 0;
       
        Color32 mainColor = GetClassColor(WeaponDataManager.Instance.Database.GetWeaponData(mainweaponID).WeaponClass);
        
        if (GameManager.instance.weaponCnt[mainweaponID - 1] > 0)
            transform.GetChild(0).GetComponent<Image>().color = mainColor;
        
        foreach (var weapon in materialWeapons) 
        {
            string weaponClassStr = WeaponDataManager.Instance.Database.GetWeaponData(weapon).WeaponClass;

            if (GameManager.instance.weaponCnt[weapon - 1] > 0)
            {
                cnt++;
                Color32 color = GetClassColor(weaponClassStr);
                transform.GetChild(i).GetComponent<Image>().color = color;
            }
            
            i += 2;
        }

        if (cnt == materialWeapons.Length)
            canCombineBorder.SetActive(true);
        else
            canCombineBorder.SetActive(false);
    }

    public void CraftWeapon()
    {
        // 갖고있는 재료의 개수를 나타내는 변수
        int hasMaterialCnt = 0;
        // 복사용 배열
        int[] tmpCnt = new int[GameManager.instance.weaponCnt.Length];
        // 갖고있는 무기 개수 배열을 복사
        Array.Copy(GameManager.instance.weaponCnt, tmpCnt, tmpCnt.Length);

       //재료 배열을 돌면서 만약에 갖고 있는게 더 많으면 복사한 배열에서 빼주고 필요한 갯수에서 빼줌
       foreach (int i in materialWeapons)
       {
            if (tmpCnt[i - 1] >= 1)
            {
                hasMaterialCnt++;
                tmpCnt[i-1]--;
            }                
       }

       // 그래서 갖고있는개수랑 있는거랑 같으면 (재료가 다있으면)
       if (hasMaterialCnt == materialWeapons.Length) 
       {
            //// 갖고있는 무기 배열에서 빼줌
            //foreach (int i in materialWeapons)
            //{
            //    GameManager.instance.weaponCnt[i - 1]--;
            //}
            // 아이템 데이터 생성
            List<int>materialsList = materialWeapons.ToList();
            InventoryItem item = new InventoryItem
            {
                image = transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite
            };
            item.AssignWeapon(mainweaponID); // 아이템에 무기데이터 넣어줌
            WeaponUI.Instance.weaponID = mainweaponID;
            
            InventoryManager.instance.AddItem(item,false);
            InventoryManager.instance.RemoveItem(materialsList, mainweaponID,item);
            
            if (InventoryManager.instance.isClassSorted)
                InventoryManager.instance.ClickClassShowButton();

            UIManager.instance.CreateCombineUI(mainweaponID);
            GameManager.instance.weaponCnt[mainweaponID - 1]++;
            GameManager.instance.UpdateUseableWeaponCnt();
            BookMakredSlotUI.Instance.UpdateAllSlot();
       }
    }

    public Color32 GetClassColor(string classStr)
    {
        Color32 color = new Color32(56, 56, 56, 255);
        
        switch(classStr)
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
