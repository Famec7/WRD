using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionUIGenerator : Singleton<InventoryDescriptionUIGenerator>
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject inventoryDescriptionUIPrefab;
    public Transform parentTransform;
    public List<GameObject> inventoryDescriptionUIList;
    public GameObject SkillIconPrefab;
    
    protected override void Init()
    {
        return;
    }
    
    void Start()
    {
        List<int> weaponNums = WeaponDataManager.Instance.Database.GetAllWeaponNums();


        for (int weaponId = 1; weaponId <= WeaponDataManager.Instance.Database.GetWeaponDataCount(); weaponId++)
        {
            var data = WeaponDataManager.Instance.GetWeaponData(weaponId);
            var inventoryDescriptionPopUpUIGameObject = Instantiate(inventoryDescriptionUIPrefab, parentTransform) as GameObject;
            InventoryDescriptionPopUpUI inventoryDescriptionPopUpUI = inventoryDescriptionPopUpUIGameObject.GetComponent<InventoryDescriptionPopUpUI>();
            inventoryDescriptionPopUpUI.weaponId = weaponId;
            inventoryDescriptionPopUpUI.weaponNameText.text = data.WeaponName;
            string weaponIconPath = "WeaponIcon/" + weaponNums[weaponId - 1].ToString();

            inventoryDescriptionPopUpUI.weaponImage.sprite = ResourceManager.Instance.Load<Sprite>(weaponIconPath);

            inventoryDescriptionPopUpUI.weaponClassText.text = WeaponDataManager.Instance.GetKorWeaponClassText(weaponId);
            inventoryDescriptionPopUpUI.weaponStatText[0].text = data.AttackDamage.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[1].text = data.AttackSpeed.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[2].text = data.AttackRange.ToString();
            inventoryDescriptionPopUpUI.weaponStatText[3].text = WeaponDataManager.Instance.GetKorWeaponTypeText(weaponId);
            //inventoryDescriptionPopUpUI.weaponTypeText.text = WeaponDataManager.Instance.GetKorWeaponRTypeText(weaponId);
            
            inventoryDescriptionUIList.Add(inventoryDescriptionPopUpUIGameObject);
            inventoryDescriptionPopUpUIGameObject.SetActive(false);

            int skillIndex = 0; // 스킬 번호는 0부터 시작
            if (SkillInfoManager.Instance.WeaponSkills.ContainsKey(weaponNums[weaponId - 1]))
            {

                var skillList = SkillInfoManager.Instance.WeaponSkills[weaponNums[weaponId - 1]];
                int weaponNum = WeaponDataManager.Instance.Database.GetWeaponNumByID(weaponId);
                foreach (var skill in skillList)
                {
                    var skillIconGameObject = Instantiate(SkillIconPrefab, inventoryDescriptionPopUpUI.transform.GetChild(2)) as GameObject;
                    skillIconGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(15 + skillIndex * 85f, 0);
                    skillIconGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(80f, 80f);
                    SkillIcon skillIcon = skillIconGameObject.GetComponent<SkillIcon>();
                    skillIcon.Init();
                    skillIcon.WeaponNum = WeaponDataManager.Instance.Database.GetWeaponNumByID(weaponId);
                    skillIcon.SkillCount = skillIndex;
                    skillIcon.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.Instance.Load<Sprite>("SkillIcon/" + weaponNum.ToString() + "_" + skillIndex.ToString());
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
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
