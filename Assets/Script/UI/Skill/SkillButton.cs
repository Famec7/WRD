using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour
{
    private Button _button;

    [SerializeField] private Image _image;
    [SerializeField]
    private ActiveSkillBase _currentSkill;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (_currentSkill != null && _currentSkill.IsCoolTime)
        {
            _image.fillAmount = _currentSkill.CurrentCoolTime / _currentSkill.Data.CoolTime;
        }
    }

    public void SetSkill(ActiveSkillBase skill)
    {
        _currentSkill = skill;
        _button.onClick.AddListener(skill.UseSkill);
    }

    public void RemoveSkill(ActiveSkillBase skill)
    {
        _currentSkill = null;
        _button.onClick.RemoveListener(skill.UseSkill);
    }
}