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
        SkillIconSelectUI = transform.parent.GetChild(1).gameObject;
        
        SkillName = SkillDescriptionTextArea.GetChild(0).GetComponent<TextMeshProUGUI>();
        SkillType = SkillDescriptionTextArea.GetChild(1).GetComponent<TextMeshProUGUI>();
        SkillDescriptionText = SkillDescriptionTextArea.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void InventoryDescriptionPopUpInit()
    {
        Transform SkillDescriptionTextArea = transform.parent.GetChild(0);
        SkillIconSelectUI = transform.parent.GetChild(1).gameObject;

        SkillName = SkillDescriptionTextArea.GetChild(0).GetComponent<TextMeshProUGUI>();
        SkillDescriptionText = SkillDescriptionTextArea.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ClickSkillIcon()
    {
        SkillIconSelectUI.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

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
