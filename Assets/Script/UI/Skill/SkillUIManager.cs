﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SkillUIManager : Singleton<SkillUIManager>
{
    protected override void Init()
    {
        _settingDropdown.onValueChanged.AddListener(OnClickSettingButton);
    }


    /********************************Active Skill UI********************************/

    #region Active Skill UI

    [SerializeField] private List<SkillButton> _skillButtons;
    private int _activeButton = 0;
    
    [SerializeField] private SkillPopup _popupPanel;

    /*
     * Active Skill Button 추가
     */
    public void AddSkillButton(ActiveSkillBase skill)
    {
        _skillButtons[_activeButton].gameObject.SetActive(true);
        _skillButtons[_activeButton].SetSkill(skill);
        _activeButton++;
    }

    /*
     * Active Skill Button 제거
     */
    public void RemoveSkillButton(ActiveSkillBase skill)
    {
        _activeButton--;
        _skillButtons[_activeButton].gameObject.SetActive(false);
        _skillButtons[_activeButton].RemoveSkill(skill);
    }

    // num번째 스킬 버튼 클릭
    private void OnClickSkillButton(int num)
    {
        var currentActiveSkill = SkillManager.Instance.GetActiveSkill(num);

        // 스킬 사용 방식에 따라 다르게 처리 -> 여기서 뭔가 할게 없어서 일단 비워둠
        switch (SettingManager.Instance.CurrentActiveSettingType)
        {
            case SettingManager.ActiveSettingType.Auto:
                // 자동 사용
                break;
            case SettingManager.ActiveSettingType.SemiAuto:
                // 반자동 사용
                break;
            case SettingManager.ActiveSettingType.Manual:
                // 수동 사용
                break;
            default:
                break;
        }

        currentActiveSkill.UseSkill();
    }

    public void ShowPopupPanel(int index = 0)
    {
        _popupPanel.ShowPopup(index);
    }

    public void ClosePopupPanel()
    {
        _popupPanel.HidePopup();
    }

    public void NextPhase()
    {
        _popupPanel.NextPhase();
    }

    #endregion

    /*******************************Skill Setting*******************************/

    #region Skill Setting
    
    [SerializeField]
    private TMP_Dropdown _settingDropdown;

    public void OnClickSettingButton(int type)
    {
#if UNITY_EDITOR
        Debug.Log("Setting Type : " + (SettingManager.ActiveSettingType)type);
#endif
        SettingManager.Instance.CurrentActiveSettingType = (SettingManager.ActiveSettingType)type;
    }

    #endregion
}