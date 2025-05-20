using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    // Start is called before the first frame update

    public Image IconImage;

    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillType;
    public TextMeshProUGUI SkillDescriptionText;
    public TextMeshProUGUI CoolTimeText;
    public Image CoolTimeImage;

    public GameObject SkillIconSelectUI;

    public int WeaponNum;
    public int SkillCount;

    public void Init()
    {

        Transform SkillDescriptionTextArea = transform.parent.GetChild(0);
        SkillDescriptionTextArea.gameObject.SetActive(true);
        SkillIconSelectUI = transform.parent.GetChild(1).gameObject;
        SkillIconSelectUI.SetActive(true);
        SkillName = SkillDescriptionTextArea.GetChild(0).GetComponent<TextMeshProUGUI>();
        SkillName.gameObject.SetActive(true);
        SkillType = SkillDescriptionTextArea.GetChild(1).GetComponent<TextMeshProUGUI>();
        SkillType.gameObject.SetActive(true);
        SkillDescriptionText = SkillDescriptionTextArea.GetChild(2).GetComponent<TextMeshProUGUI>();
        int childCount = SkillDescriptionTextArea.transform.childCount;
        CoolTimeText = SkillDescriptionTextArea.GetChild(4).GetComponent<TextMeshProUGUI>();
        CoolTimeImage = SkillDescriptionTextArea.GetChild(3).GetComponent<Image>();

        SkillDescriptionText.gameObject.SetActive(true);
        if (SkillType.text == "액티브")
        {
            CoolTimeText.gameObject.SetActive(true);
            CoolTimeImage.gameObject.SetActive(true);
        }
        else
        {
            CoolTimeText.gameObject.SetActive(false);
            CoolTimeImage.gameObject.SetActive(false);
        }
    }

    public void InventoryDescriptionPopUpInit()
    {
        Transform SkillDescriptionTextArea = transform.parent.GetChild(0);
        SkillIconSelectUI = transform.parent.GetChild(1).gameObject;

        SkillName = SkillDescriptionTextArea.GetChild(0).GetComponent<TextMeshProUGUI>();
        SkillName.gameObject.SetActive(true);
        SkillDescriptionText = SkillDescriptionTextArea.GetChild(1).GetComponent<TextMeshProUGUI>();
        SkillDescriptionText.gameObject.SetActive(true);
    }

    public void ClickSkillIcon()
    {
        SkillIconSelectUI.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        SkillIconSelectUI.SetActive(true);
        CoolTimeImage.gameObject.SetActive(true);

        if (SkillInfoManager.Instance.WeaponSkills.ContainsKey(WeaponNum))
        {
            var skills = SkillInfoManager.Instance.WeaponSkills[WeaponNum];

            SkillName.text = skills[SkillCount].Name; 
            if(SkillType != null)
                SkillType.text = skills[SkillCount].Type;
            SkillDescriptionText.text = skills[SkillCount].Info;

            if (SkillType.text == "액티브")
            {
                CoolTimeText.text = skills[SkillCount].CoolTime;
                CoolTimeImage.gameObject.SetActive(true);
                CoolTimeText.gameObject.SetActive(true);
            }
            else
            {
                CoolTimeText.gameObject.SetActive(false);
                CoolTimeImage.gameObject.SetActive(false);
            }
        }
    }
}
