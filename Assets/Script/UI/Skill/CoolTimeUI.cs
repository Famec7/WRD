using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoolTimeUI : MonoBehaviour
{
    private TextMeshProUGUI _coolTimeText;
    private ActiveSkillBase _currentData;

    private Image _panelImage;

    private const float CriticalCoolTimeThreshold = 5.0f;

    private void Awake()
    {
        if (TryGetComponent(out TextMeshProUGUI text))
        {
            _coolTimeText = text;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 컴포넌트가 없습니다.");
        }

        _panelImage = GetComponentInChildren<Image>();

        HideCoolTime();
    }

    public void SetSkill(ActiveSkillBase skill)
    {
        _currentData = skill;

        if (_currentData != null)
        {
            ShowCoolTime();
        }
        else
        {
            HideCoolTime();
        }
    }

    private void Update()
    {
        if (_currentData == null)
        {
            return;
        }

        if (_currentData.CurrentCoolTime > 0f)
        {
            ShowCoolTime();
        }
        else
        {
            HideCoolTime();
        }
    }

    private void ShowCoolTime()
    {
        _panelImage.enabled = true;

        _coolTimeText.text = $"{_currentData.CurrentCoolTime:F0}";
        _coolTimeText.color = _currentData.CurrentCoolTime <= CriticalCoolTimeThreshold ? Color.red : Color.black;
    }

    private void HideCoolTime()
    {
        _panelImage.enabled = false;
        _coolTimeText.text = string.Empty;
    }
}