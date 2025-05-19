using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    private bool _isSettingUIOpen = false;
    private bool _isPause = false;
    
    [SerializeField] private GameObject _settingUI;
    public Image PauseButton;
    public Sprite PauseSprite;
    public Sprite ResumeSprite;

    private void OpenSettingUI()
    {
        _isSettingUIOpen = true;
        _isPause = true;
        
        Time.timeScale = 0;
        
        _settingUI.SetActive(true);
        PauseButton.sprite = ResumeSprite;
    }

    private void CloseSettingUI()
    {
        _isSettingUIOpen = false;
        _isPause = false;
        
        Time.timeScale = 1;
        
        _settingUI.SetActive(false);
        PauseButton.sprite = PauseSprite;
    }
    
    public void OnClickPauseButton()
    {
        if (_isPause)
        {
            CloseSettingUI();
        }
        else
        {
            OpenSettingUI();
        }
    }
}
