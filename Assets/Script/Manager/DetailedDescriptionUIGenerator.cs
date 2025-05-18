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
    public GameObject SkillIconPrefab;

    protected override void Init()
    {
        return;
    }
    
    void Start()
    {
        int weaponDataCount = WeaponDataManager.Instance.Database.GetWeaponDataCount();
        List<int> weaponNums = WeaponDataManager.Instance.Database.GetAllWeaponNums();
        for (int weaponId = 1; weaponId <= weaponDataCount; weaponId++)
        {
            int weaponNum = WeaponDataManager.Instance.Database.GetWeaponNumByID(weaponId);
            int canCombineCnt = 0;
            var detailedDescriptionUIGameObject = Instantiate(detailedDescriptionUIPrefab, parentTransform) as GameObject;
            DetailedDescriptionUI detailedDescriptionUI = detailedDescriptionUIGameObject.GetComponent<DetailedDescriptionUI>();
            List<int> canCombinWeaponsList = new List<int>();
            detailedDescriptionUIList.Add(detailedDescriptionUIGameObject);
            
            for (int j = 0; j < weaponDataCount; j++)
            {
                string mainCombi = WeaponDataManager.Instance.Database.GetWeaponData(j + 1).MainCombi;
                if (mainCombi == weaponNum.ToString())
                {
                    canCombinWeaponsList.Add(WeaponDataManager.Instance.Database.GetWeaponData(j + 1).ID);
                }
            }
            canCombineCnt = canCombinWeaponsList.Count;

            detailedDescriptionUI.weaponNameText.text = WeaponDataManager.Instance.Database.GetWeaponData(weaponId).WeaponName;
            string weaponIconPath = "WeaponIcon/" + weaponNums[weaponId-1].ToString();

            detailedDescriptionUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);
            detailedDescriptionUI.weaponClassText.text = WeaponDataManager.Instance.GetKorWeaponClassText(weaponId);

            int skillIndex = 0; // 스킬 번호는 0부터 시작
            if (SkillInfoManager.Instance.WeaponSkills.ContainsKey(weaponNums[weaponId - 1]))
            {
                
                var skillList = SkillInfoManager.Instance.WeaponSkills[weaponNums[weaponId - 1]];

                foreach (var skill in skillList)
                {
                    
                    var skillIconGameObject = Instantiate(SkillIconPrefab, detailedDescriptionUI.transform.GetChild(1)) as GameObject;
                    skillIconGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-280 + skillIndex * 120f, 204.2f);
                    
                    SkillIcon skillIcon = skillIconGameObject.GetComponent<SkillIcon>();
                    skillIcon.Init();
                    skillIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>("SkillIcon/" + weaponNum.ToString() + "_" + skillIndex.ToString());
                    skillIcon.WeaponNum = weaponNums[weaponId - 1];
                    skillIcon.SkillCount = skillIndex;

                    // UI 업데이트 (스킬 정보 적용)
                    if (skillIndex == 0)
                    {
                        skillIcon.SkillIconSelectUI.GetComponent<RectTransform>().anchoredPosition = skillIcon.GetComponent<RectTransform>().anchoredPosition;
                        skillIcon.SkillName.text = skill.Name;
                        skillIcon.SkillType.text = skill.Type;
                        skillIcon.SkillDescriptionText.text = skill.Info;
                    }
                    skillIndex++;
                }
            }


            int idx = 0;

            foreach (var highLevelweaponID in canCombinWeaponsList)
            {
                var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -170);
                highLevelWeaponIcon.GetComponent<Image>().color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.GetWeaponData(highLevelweaponID).tier);
                highLevelWeaponIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(highLevelweaponID).WeaponName;
                var path = "WeaponIcon/" + WeaponDataManager.Instance.GetWeaponData(highLevelweaponID).num;
                highLevelWeaponIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                highLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(highLevelweaponID));
                idx++;
            }

            idx = 0;
            //최상위
            for (int j = 0; j < weaponDataCount; j++)
            {
                foreach (var highLevelweaponID in canCombinWeaponsList)
                {
                    if (WeaponDataManager.Instance.Database.GetWeaponData(j + 1).MainCombi == WeaponDataManager.Instance.Database.GetWeaponNumByID(highLevelweaponID).ToString())
                    {
                        var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.transform) as GameObject;
                        highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -350);
                        highLevelWeaponIcon.GetComponent<Image>().color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.GetWeaponData(highLevelweaponID).tier);
                        highLevelWeaponIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(j).WeaponName;
                        var path = "WeaponIcon/" + WeaponDataManager.Instance.GetWeaponData(j).num; 
                        highLevelWeaponIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);
                        highLevelWeaponIcon.transform.GetComponent<Button>().onClick.AddListener(()=>UIManager.instance.CreateDetailedDescriptionUI(j));
                        idx++;
                    }
                }
            }
            
            

            detailedDescriptionUIGameObject.SetActive((false));
        }
    }
}
