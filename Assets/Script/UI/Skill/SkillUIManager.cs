using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SkillUIManager : Singleton<SkillUIManager>
{
    protected override void Init()
    {
        _settingDropdown.onValueChanged.AddListener(OnClickSettingButton);
    }


    /********************************Skill UI********************************/

    #region Skill UI

    [SerializeField] private List<SkillButton> _skillButtons;
    private int _activeButton = 0;
    
    [SerializeField] private SkillPopup _popupPanel;

    /*
     * kill Button 추가
     */
    public void AddSkillButton(SkillBase skill)
    {
        _skillButtons[_activeButton].SetSkill(skill);
        _activeButton++;
        
        if (_activeButton >= _skillButtons.Count)
        {
            _activeButton = _skillButtons.Count - 1;
        }
    }

    /*
     * Skill Button 제거
     */
    public void RemoveSkillButton(SkillBase skill)
    {
        _activeButton--;
        
        if (_activeButton < 0)
        {
            _activeButton = 0;
        }
        
        _skillButtons[_activeButton].RemoveSkill(skill);
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