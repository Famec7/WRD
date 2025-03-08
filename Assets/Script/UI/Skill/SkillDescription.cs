using System;
using TMPro;
using UnityEngine;

public class SkillDescription : MonoBehaviour
{
    private SkillBase _skill;
    
    [SerializeField]
    private TextMeshProUGUI _skillName;
    [SerializeField]
    private TextMeshProUGUI _description;

    private void Awake()
    {
        _skillName.fontStyle = FontStyles.Bold;
    }

    public void SetSkill(SkillBase skill)
    {
        _skill = skill;
        
        ActiveSkillBase activeSkill = _skill as ActiveSkillBase;
        
        if (activeSkill != null)
        {
            _skillName.text = activeSkill.Data.Name;
            _description.text = activeSkill.Data.Description;
            return;
        }
        
        PassiveSkillBase passiveSkill = _skill as PassiveSkillBase;
        
        if (passiveSkill != null)
        {
            _skillName.text = passiveSkill.Data.Name;
            _description.text = passiveSkill.Data.Description;
            return;
        }
        
        PassiveAuraSkillBase passiveAuraSkill = _skill as PassiveAuraSkillBase;
        
        if (passiveAuraSkill != null)
        {
            _skillName.text = passiveAuraSkill.Data.Name;
            _description.text = passiveAuraSkill.Data.Description;
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