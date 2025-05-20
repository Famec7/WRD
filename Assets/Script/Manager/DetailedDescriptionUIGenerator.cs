using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                var data = WeaponDataManager.Instance.Database.GetWeaponData(j + 1);
                string main = data.MainCombi;
                string combi = data.Combi ?? "";

                bool isMain = main == weaponNum.ToString();
                bool inRecipe = combi
                    .Split(' ')
                    .Where(s => int.TryParse(s, out _))
                    .Select(int.Parse)
                    .Contains(weaponNum);

                if (isMain || inRecipe)
                    canCombinWeaponsList.Add(data.ID);
            }
            canCombineCnt = canCombinWeaponsList.Count;

            detailedDescriptionUI.weaponNameText.text = WeaponDataManager.Instance.Database.GetWeaponData(weaponId).WeaponNameKR;
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
                    skillIcon.WeaponNum = weaponNums[weaponId - 1];

                    skillIcon.Init();
                    skillIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>("SkillIcon/" + weaponNum.ToString() + "_" + skillIndex.ToString());
                    if (skillIcon.WeaponNum == 0) continue;

                    skillIcon.SkillCount = skillIndex;

                    // UI 업데이트 (스킬 정보 적용)
                    if (skillIndex == 0)
                    {
                        skillIcon.SkillIconSelectUI.GetComponent<RectTransform>().anchoredPosition = skillIcon.GetComponent<RectTransform>().anchoredPosition;
                        skillIcon.SkillName.text = skill.Name;
                        skillIcon.SkillType.text = skill.Type;
                        skillIcon.CoolTimeText.text = skill.CoolTime;
                        skillIcon.SkillDescriptionText.text = skill.Info;
                    }
                    skillIndex++;
                }
            }


            int idx = 0;

            foreach (var highLevelweaponID in canCombinWeaponsList)
            {
                var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.HighLevelTransform.transform) as GameObject;
                highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -170);
                highLevelWeaponIcon.GetComponent<Image>().color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.GetWeaponData(highLevelweaponID).tier);
                highLevelWeaponIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(highLevelweaponID).WeaponNameKR;
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
                    var data = WeaponDataManager.Instance.Database.GetWeaponData(j + 1);
                    if (data.MainCombi == WeaponDataManager.Instance.Database.GetWeaponNumByID(highLevelweaponID).ToString())
                    {
                        int capturedJ = j;
                        int capturedHighID = highLevelweaponID;

                        var highLevelWeaponIcon = Instantiate(highLevelCombinationPrefab, detailedDescriptionUI.HighHighLevelTransform.transform) as GameObject;
                        highLevelWeaponIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-280 + 120 * idx, -350);
                        highLevelWeaponIcon.GetComponent<Image>().color = WeaponTierTranslator.GetClassColor(WeaponDataManager.Instance.GetWeaponData(capturedHighID).tier);
                        highLevelWeaponIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = WeaponDataManager.Instance.Database.GetWeaponData(capturedJ).WeaponNameKR;
                       
                        string path = "WeaponIcon/" +WeaponDataManager.Instance.Database.GetWeaponData(capturedJ).num;
                        highLevelWeaponIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>(path);

                        var button = highLevelWeaponIcon.GetComponent<Button>();

                        button.onClick.AddListener(() => { UIManager.instance.CreateDetailedDescriptionUI(capturedJ);});
                        idx++;
                    }
                }
            }



            detailedDescriptionUIGameObject.SetActive((false));
        }
    }
}
