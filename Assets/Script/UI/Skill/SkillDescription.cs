using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour
{
    private SkillData _skill;
    
    [SerializeField]
    private TextMeshProUGUI _skillName;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private TextMeshProUGUI _skillCoolTime;
    [SerializeField]
    private Image _coolTimeIcon;

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
                _skillCoolTime.text = activeData.CoolTime.ToString();
                _skillCoolTime.gameObject.SetActive(true);
                _coolTimeIcon.gameObject.SetActive(true);
                return;
            case PassiveSkillData passiveData:
                _skillName.text = passiveData.Name;
                _description.text = passiveData.Description;
                _skillCoolTime.gameObject.SetActive(false);
                _coolTimeIcon.gameObject.SetActive(false);
                return;
            case PassiveAuraSkillData passiveAuraData:
                _skillName.text = passiveAuraData.Name;
                _description.text = passiveAuraData.Description;
                _skillCoolTime.gameObject.SetActive(false);
                _coolTimeIcon.gameObject.SetActive(false);
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