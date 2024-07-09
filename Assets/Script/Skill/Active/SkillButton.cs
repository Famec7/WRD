using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button skillButton;
    public ActiveSkillBase activeSkill;

    void Start()
    {
        if (skillButton != null && activeSkill != null)
        {
            // 버튼 클릭 시 Execute 메서드 호출
            skillButton.onClick.AddListener(() => {activeSkill.buttonClicked = true;});
        }
    }
}
