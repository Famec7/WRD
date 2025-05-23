using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponCombinationUIGenerator : Singleton<WeaponCombinationUIGenerator>
{
    // Start is called before the first frame update
    public GameObject   combinationUIPrefab;
    public GameObject[] subCombinationPrefab;
    public Transform parentTransform;
    public List<GameObject> combineWeaponUIList;
    protected override void Init()
    {
        return;
    }
    void Start()
    {
        combineWeaponUIList = new List<GameObject>();
        List<int> weaponNums = WeaponDataManager.Instance.Database.GetAllWeaponNums();

        int weaponDataCount = WeaponDataManager.Instance.Database.GetWeaponDataCount();
        for (int i =1; i <= weaponDataCount; i++)
            ResourceManager.Instance.Load<Sprite>("WeaponIcon/"+ weaponNums[i-1].ToString());

        for (int i = 1; i <= weaponDataCount; i++)
        { 
            List<int> canCombinWeaponsList = new List<int>();
            int canCombineCnt = 0;
             
            for (int j = 5; j < weaponDataCount-1; j++)
            {
                var data = WeaponDataManager.Instance.GetWeaponData(j + 1);
                 string combi = data.Combi;
                 string[] combis = combi.Split('\x020');
                 string mainCombi = data.MainCombi;
                 int mainCombiID = WeaponDataManager.Instance.Database.GetWeaponIdByNum(Convert.ToInt32(mainCombi));    
                 if (mainCombiID == i)
                 {
                     foreach (string s in combis)
                     {
                         if (s == "") continue;
                         int id = Convert.ToInt32(s);
                         if (id == WeaponDataManager.Instance.GetWeaponData(i).num)
                         {
                             canCombinWeaponsList.Add(data.ID);
                             break;
                         }
                     }
                 }
            }

            var combinationUI = Instantiate(combinationUIPrefab, parentTransform) as GameObject;
            combinationUI.GetComponent<CombinePopUpUI>().weaponId = i;
            combineWeaponUIList.Add((combinationUI));
            canCombineCnt = canCombinWeaponsList.Count;
            var weaponName = combinationUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            weaponName.text = WeaponDataManager.Instance.Database.GetWeaponData(i).WeaponNameKR;

            var weaponIcon = combinationUI.transform.GetChild(0).GetComponent<Image>();
            string weaponIconPath = "WeaponIcon/" + weaponNums[i-1].ToString();
            weaponIcon.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            
            combinationUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(239, -780, 0);
            combinationUI.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 570 + (canCombineCnt - 2) * 80);
            
            for (int j = 0; j < canCombineCnt; j++)
            {
                int targetCode = canCombinWeaponsList[j];
                string combi = WeaponDataManager.Instance.Database.GetWeaponData(targetCode).Combi;
                string[] combis = combi.Split('\x020');

                var subCombi = Instantiate(subCombinationPrefab[combis.Length - 2], combinationUI.transform) as GameObject;
                subCombi.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                subCombi.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 1f);
                subCombi.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

                subCombi.GetComponent<RectTransform>().anchoredPosition = new Vector3(-220, -300 - 150 * j, -60);
                subCombi.GetComponent<SubCombinationUI>().originPosition =new Vector3(-220, -300 - 150 * j, -60);
                subCombi.GetComponent<SubCombinationUI>().mainweaponID = targetCode;

                string path = "WeaponIcon/" + WeaponDataManager.Instance.Database.GetWeaponData(targetCode).num.ToString();
                subCombi.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                subCombi.GetComponent<SubCombinationUI>().materialWeapons = new int[combis.Length];
                int index = 2;
                
                foreach (string s in combis)
                {
                    int materialCode = Convert.ToInt32(s);
                    path = "WeaponIcon/" + materialCode.ToString();
                    subCombi.GetComponent<SubCombinationUI>().materialWeapons[index / 2 - 1] = materialCode;
                    //Debug.Log(index);
                    subCombi.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                    index +=2;
                }

                subCombi.GetComponent<SubCombinationUI>().SetCraftButtons();
            }
            
            combinationUI.SetActive((false));
        }
      
    }

}
