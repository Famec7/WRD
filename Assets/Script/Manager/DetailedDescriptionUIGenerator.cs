using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailedDescriptionUIGenerator : Singleton<DetailedDescriptionUIGenerator>
{
    // Start is called before the first frame update
    public GameObject detailedDescriptionUIPrefab;
    public GameObject highLevelCombinationPrefab;
    public Transform parentTransform;
    public List<GameObject> detailedDescriptionUIList;

    protected override void Init()
    {
        return;
    }
    
    void Start()
    {
        int weaponDataCount = WeaponDataManager.Instance.Database.GetWeaponDataCount();
        
        for (int weaponId = 1; weaponId <= weaponDataCount; weaponId++)
        {
            int canCombineCnt = 0;
            var detailedDescriptionUIGameObject = Instantiate(detailedDescriptionUIPrefab, parentTransform) as GameObject;
            DetailedDescriptionUI detailedDescriptionUI = detailedDescriptionUIGameObject.GetComponent<DetailedDescriptionUI>();
            List<int> canCombinWeaponsList = new List<int>();
            detailedDescriptionUIList.Add(detailedDescriptionUIGameObject);
            
            for (int j = 0; j < weaponDataCount; j++)
            {
                string mainCombi = WeaponDataManager.Instance.Database.GetWeaponData(j + 1).MainCombi;
                if (mainCombi == weaponId.ToString())
                {
                    canCombinWeaponsList.Add(WeaponDataManager.Instance.Database.GetWeaponData(j + 1).ID);
                }
            }
            canCombineCnt = canCombinWeaponsList.Count;

            detailedDescriptionUI.weaponNameText.text = WeaponDataManager.Instance.Database.GetWeaponData(weaponId).WeaponName;
            string weaponIconPath = "WeaponIcon/" + weaponId.ToString();

            detailedDescriptionUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            detailedDescriptionUI.weaponClassText.text = WeaponDataManager.Instance.GetKorWeaponClassText(weaponId);

            int idx = 0;

            foreach (var highLevelweaponID in canCombinWeaponsList)
            {
                var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -280);
                highLevelWeaponIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(highLevelweaponID).WeaponName;
                var path = "WeaponIcon/" + highLevelweaponID;
                highLevelWeaponIcon.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                highLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(highLevelweaponID));
                idx++;
            }

            idx = 0;
            
            for (int j = 0; j < weaponDataCount; j++)
            {
                
                foreach (var highLevelweaponID in canCombinWeaponsList)
                {
                    if (WeaponDataManager.Instance.Database.GetWeaponData(j + 1).MainCombi == highLevelweaponID.ToString())
                    {
                        var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                        highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -420);
                        highLevelWeaponIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(j).WeaponName;
                        var path = "WeaponIcon/" + j;
                        highLevelWeaponIcon.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                        highLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(highLevelweaponID));
                        idx++;
                    }
                }
            }
            
            detailedDescriptionUIGameObject.SetActive((false));
        }
    }
}
