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
        
        for (int weaponId = 1; weaponId <= WeaponDataManager.instance.Data.Length; weaponId++)
        {
            int canCombineCnt = 0;
            var detailedDescriptionUIGameObject = Instantiate(detailedDescriptionUIPrefab, parentTransform) as GameObject;
            DetailedDescriptionUI detailedDescriptionUI = detailedDescriptionUIGameObject.GetComponent<DetailedDescriptionUI>();
            List<int> canCombinWeaponsList = new List<int>();
            detailedDescriptionUIList.Add(detailedDescriptionUIGameObject);
            
            for (int j = 0; j < WeaponDataManager.instance.Data.Length; j++)
            {
                string mainCombi = WeaponDataManager.instance.Data[j].mainCombi;
                if (mainCombi == weaponId.ToString())
                {
                    canCombinWeaponsList.Add(WeaponDataManager.instance.Data[j].id);
                }
            }
            canCombineCnt = canCombinWeaponsList.Count;

            detailedDescriptionUI.weaponNameText.text = WeaponDataManager.instance.Data[weaponId - 1].weaponName;
            string weaponIconPath = "WeaponIcon/" + weaponId.ToString();

            detailedDescriptionUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            detailedDescriptionUI.weaponClassText.text = WeaponDataManager.instance.GetKorWeaponClassText(weaponId);

            int idx = 0;

            foreach (var highLevelweaponID in canCombinWeaponsList)
            {
                var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -280);
                highLevelWeaponIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.instance.Data[highLevelweaponID - 1].weaponName;
                var path = "WeaponIcon/" + highLevelweaponID;
                highLevelWeaponIcon.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                highLevelWeaponIcon.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(highLevelweaponID));
                idx++;
            }

            idx = 0;
            
            for (int j = 0; j < WeaponDataManager.instance.Data.Length; j++)
            {
                
                foreach (var highLevelweaponID in canCombinWeaponsList)
                {
                    if (WeaponDataManager.instance.Data[j].mainCombi == highLevelweaponID.ToString())
                    {
                        var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                        highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -420);
                        highLevelWeaponIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.instance.Data[j - 1].weaponName;
                        var path = "WeaponIcon/" + j;
                        highLevelWeaponIcon.GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                        highLevelWeaponIcon.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(highLevelweaponID));
                        idx++;
                    }
                }
            }
            
            detailedDescriptionUIGameObject.SetActive((false));
        }
    }
}
