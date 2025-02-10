using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailedCombinationPopupUIGenerator : Singleton<DetailedCombinationPopupUIGenerator>
{
    public GameObject DetailedCombinationPopupUIPrefab;
    public GameObject highLevelCombinationPrefab;
    public Transform parentTransform;
    public List<GameObject> DetailedCombinationPopupUIList;

    void Start()
    {
        int weaponDataCount = WeaponDataManager.Instance.Database.GetWeaponDataCount();
        List<int> weaponNums = WeaponDataManager.Instance.Database.GetAllWeaponNums();
        for (int weaponId = 1; weaponId <= weaponDataCount; weaponId++)
        {
            int weaponNum = WeaponDataManager.Instance.Database.GetWeaponNumByID(weaponId);
            int canCombineCnt = 0;
            var detailedDescriptionUIGameObject = Instantiate(DetailedCombinationPopupUIPrefab, parentTransform) as GameObject;
            DetailedCombinationPopupUI detailedCombinationPopupUI = detailedDescriptionUIGameObject.GetComponent<DetailedCombinationPopupUI>();
            List<int> canCombinWeaponsList = new List<int>();
            DetailedCombinationPopupUIList.Add(detailedDescriptionUIGameObject);
            detailedDescriptionUIGameObject.SetActive(false);
            detailedDescriptionUIGameObject.transform.GetChild(2).GetComponent<DetailedCombinationButton>().weaponID = weaponId;
            detailedCombinationPopupUI.Init(weaponNum);

            for (int j = 1; j < weaponDataCount; j++)
            {
                var highWeaponData = WeaponDataManager.Instance.GetWeaponData(j);
                string highWeaponCombi = highWeaponData.Combi;
                string[] highWeaponcombis = highWeaponCombi.Split('\x020');
                foreach (var num in highWeaponcombis)
                {
                    if (num == weaponNum.ToString())
                    {
                        canCombinWeaponsList.Add(WeaponDataManager.Instance.Database.GetWeaponData(j).ID);
                    }
                }
            }
            canCombineCnt = canCombinWeaponsList.Count;
            canCombinWeaponsList = canCombinWeaponsList.Distinct().ToList();
            int idx = 0;

            foreach (var highLevelweaponID in canCombinWeaponsList)
            {
                var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedCombinationPopupUI.transform.GetChild(0)) as GameObject;
                highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-110 + 110 * idx, 0);
                var path = "WeaponIcon/" + WeaponDataManager.Instance.GetWeaponData(highLevelweaponID).num;
                highLevelWeaponIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                highLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(() => UIManager.instance.CreateDetailedCombinationPopupUI(highLevelweaponID));
                highLevelWeaponIcon.GetComponent<DetailedCombinationButton>().weaponID = highLevelweaponID;
                idx++;
            }

            var data = WeaponDataManager.Instance.GetWeaponData(weaponId);
            if (data == null || weaponId < 6) continue;
            string combi = data.Combi;
            
            string[] combis = combi.Split('\x020');
            int idx2 = 0;
            foreach (var lowLevelNum in combis)
            {
                var lowLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedCombinationPopupUI.transform.GetChild(3)) as GameObject;
                lowLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-110 + 110 * idx2, 0);
                var path = "WeaponIcon/" + lowLevelNum;
                int lowLevelID = WeaponDataManager.Instance.Database.GetWeaponIdByNum(Int32.Parse(lowLevelNum));
                lowLevelWeaponIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                if (lowLevelID  > 5)
                    lowLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(() => UIManager.instance.CreateDetailedCombinationPopupUI(lowLevelID));
                idx2++;

                lowLevelWeaponIcon.GetComponent<DetailedCombinationButton>().weaponID = lowLevelID;
            }
        }
    }
    protected override void Init()
    {
       
    }
}
