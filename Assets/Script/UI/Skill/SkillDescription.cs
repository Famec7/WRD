using System;
using TMPro;
using UnityEngine;

public class SkillDescription : MonoBehaviour
{
    private SkillData _skill;
    
    [SerializeField]
    private TextMeshProUGUI _skillName;
    [SerializeField]
    private TextMeshProUGUI _description;

    private void Awake()
    {
        _skillName.fontStyle = FontStyles.Bold;
    }

    public void SetSkill(SkillData skill)
    {
        _skill = skill;

        switch (_skill)
        {
            case ActiveSkillData activeData:
                _skillName.text = activeData.Name;
                _description.text = activeData.Description;
                return;
            case PassiveSkillData passiveData:
                _skillName.text = passiveData.Name;
                _description.text = passiveData.Description;
                return;
            case PassiveAuraSkillData passiveAuraData:
                _skillName.text = passiveAuraData.Name;
                _description.text = passiveAuraData.Description;
                return;
        }
    }
    
    public void ShowDescription()
    {
        gameObject.SetActive(true);
    }
    
    public void HideDescription()
    {
        gameObject.SetActive(false);
    }
}