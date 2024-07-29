using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : Singleton<SkillUIManager>
{
    protected override void Init()
    {
        ;
    }

    [SerializeField] private SkillPopup _popupPanel;

    /********************************Active Skill UI********************************/
    #region Active Skill UI
    
    [SerializeField] private List<SkillButton> _skillButtons = new();
    private int _activedButton = 0;

    /*
     * Active Skill Button 추가
     */
    public void AddSkillButton(ActiveSkillBase skill)
    {
        _skillButtons[_activedButton].gameObject.SetActive(true);
        _skillButtons[_activedButton].SetSkill(skill);
        _activedButton++;
    }

    /*
     * Active Skill Button 제거
     */
    public void RemoveSkillButton(ActiveSkillBase skill)
    {
        _skillButtons[_activedButton].gameObject.SetActive(false);
        _skillButtons[_activedButton].RemoveSkill(skill);
        _activedButton--;
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
    
    public void OnClickSettingButton(int type)
    {
        SettingManager.Instance.CurrentActiveSettingType = (SettingManager.ActiveSettingType)type;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickSettingButton(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickSettingButton(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickSettingButton(2);
        }
    }
    
    #endregion
}