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
        
        for (int i =1; i <= WeaponDataManager.instance.Data.Length; i++)
            ResourceManager.Instance.Load<Sprite>("WeaponIcon/"+i.ToString());

        for (int i = 1; i <= WeaponDataManager.instance.Data.Length; i++)
        { 
            List<int> canCombinWeaponsList = new List<int>();
            int canCombineCnt = 0;
             
            for (int j = 0; j < WeaponDataManager.instance.Data.Length; j++)
            {
                 string combi = WeaponDataManager.instance.Data[j].combi;
                 string[] combis = combi.Split('\x020');
                 string mainCombi = WeaponDataManager.instance.Data[j].mainCombi;
                 
                 if (mainCombi == i.ToString())
                 {
                     foreach (string s in combis)
                     {
                         if (s == "") continue;
                         int id = Convert.ToInt32(s);
                         if (id == i)
                         {
                             canCombinWeaponsList.Add(WeaponDataManager.instance.Data[j].id);
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
            weaponName.text = WeaponDataManager.instance.Data[i - 1].weaponName;

            var weaponIcon = combinationUI.transform.GetChild(0).GetComponent<Image>();
            string weaponIconPath = "WeaponIcon/" + i.ToString();
            weaponIcon.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            
            combinationUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(239, -780, 0);
            combinationUI.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 570 + (canCombineCnt - 2) * 80);
            
            for (int j = 0; j < canCombineCnt; j++)
            {
                int targetCode = canCombinWeaponsList[j];
                string combi = WeaponDataManager.instance.Data[targetCode - 1].combi;
                string[] combis = combi.Split('\x020');

                var subCombi = Instantiate(subCombinationPrefab[combis.Length - 2], combinationUI.transform) as GameObject;
                subCombi.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
                subCombi.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
                subCombi.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

                subCombi.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -230 - 150 * j, 0);
                subCombi.GetComponent<SubCombinationUI>().originPosition =new Vector3(0, -230 - 150 * j, 0);
                subCombi.GetComponent<SubCombinationUI>().mainweaponID = targetCode;

                string path = "WeaponIcon/" + targetCode.ToString();
                subCombi.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                subCombi.GetComponent<SubCombinationUI>().materialWeapons = new int[combis.Length];
                int index = 2;
                
                Array.Sort(combis, (x, y) =>
                {
                    if (x == i.ToString()) return -1;
                    else if (y == i.ToString()) return 1;
                    else return string.Compare(x, y);
                });
            
                foreach (string s in combis)
                {
                    int materialCode = Convert.ToInt32(s);
                    path = "WeaponIcon/" + s;
                    subCombi.GetComponent<SubCombinationUI>().materialWeapons[index / 2 - 1] = materialCode;
                    //Debug.Log(index);
                    subCombi.transform.GetChild(index).GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                    index +=2;
                }
            }
            
            combinationUI.SetActive((false));
        }
      
    }

}
