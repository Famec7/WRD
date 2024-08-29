using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
{
    private bool _isSettingUIOpen = false;
    private bool _isPause = false;
    
    [SerializeField] private GameObject _settingUI;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isSettingUIOpen)
            {
                CloseSettingUI();
            }
            else
            {
                OpenSettingUI();
            }
        }
    }
    
    private void OpenSettingUI()
    {
        _isSettingUIOpen = true;
        _isPause = true;
        
        Time.timeScale = 0;
        
        _settingUI.SetActive(true);
    }
    
    private void CloseSettingUI()
    {
        _isSettingUIOpen = false;
        _isPause = false;
        
        Time.timeScale = 1;
        
        _settingUI.SetActive(false);
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
