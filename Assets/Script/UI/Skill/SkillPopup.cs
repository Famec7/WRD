﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPopup : MonoBehaviour
{
    [SerializeField] private Image _panelImage;
    [SerializeField] private TMP_Text _guideText;

    [SerializeField] private List<string> _guideTexts;
    private int _curGuideTextIndex = 0;
    
    public void ShowPopup(int index = 0)
    {
        _panelImage.gameObject.SetActive(true);
        _guideText.gameObject.SetActive(true);
        
        _curGuideTextIndex = index;
        
        NextPhase();
    }
    
    public void HidePopup()
    {
        _panelImage.gameObject.SetActive(false);
        _guideText.gameObject.SetActive(false);

        _curGuideTextIndex = -1;
    }
    
    public void NextPhase()
    {
        // 마지막 텍스트일 경우 텍스트 창을 닫음
        if (_curGuideTextIndex >= _guideTexts.Count)
        {
            _guideText.gameObject.SetActive(false);
            return;
        }
        
        _guideText.text = _guideTexts[_curGuideTextIndex];
        _curGuideTextIndex++;
    }
}