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
    public GameObject SkillIconSelectUI;

    public int WeaponID;
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
        SkillDescriptionText.gameObject.SetActive(true);
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
        if (SkillInfoManager.Instance.WeaponSkills.ContainsKey(WeaponID))
        {
            var skills = SkillInfoManager.Instance.WeaponSkills[WeaponID];

            SkillName.text = skills[SkillCount].Name; 
            if(SkillType != null)
                SkillType.text = skills[SkillCount].Type;
            SkillDescriptionText.text = skills[SkillCount].Info;
        }
    }
}
