using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Fohn : ActiveSkillBase
{
    public GameObject noticePrefab;
    public Transform canvasTransform;
    protected Vector3 textPos;
    protected GameObject nP;
    protected GameObject target = null;      
    

    # region Idle State

    protected override void OnIdleEnter(ActiveSkillBase skill)
    {}
    protected override void OnIdleExecute(ActiveSkillBase skill)
    {
        if(buttonClicked)
        {
            _fsm.ChangeState(_castingState);
        }
    }
    protected override void OnIdleExit(ActiveSkillBase skill)
    {
    }

    # endregion

    # region Casting State

    protected override void OnCastingEnter(ActiveSkillBase skill)
    {
        nP.transform.localPosition = textPos;
        nP.GetComponent<Text>().text = "적을 지정하세요.";
    }
    protected override void OnCastingExecute(ActiveSkillBase skill)
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 TouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var pos = TouchPos;
            pos.z = 0f;
            TouchPos = pos;

            Collider2D col = Physics2D.OverlapPoint(TouchPos);
            // 몬스터 클릭 시 추적
            if (col && (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission")))
            {
                target = col.gameObject;
                Destroy(nP, 0f);
            }
            else
            {
                nP.GetComponent<Text>().text = "올바른 적을 지정하세요.";
                Destroy(nP, 3f);
            }
        }
    }
    protected override void OnCastingExit(ActiveSkillBase skill)
    {
        buttonClicked = false;
        if(target)
            _fsm.ChangeState(_activeState);
        else
            _fsm.ChangeState(_idleState);
    }

    # endregion
    
    # region CoolTime State
    
    protected override void OnCoolTimeEnter(ActiveSkillBase skill)
    {}
    protected override void OnCoolTimeExecute(ActiveSkillBase skill)
    {}
    protected override void OnCoolTimeExit(ActiveSkillBase skill)
    {}
    
    # endregion
    
    # region Active State
    
    protected override void OnActiveEnter(ActiveSkillBase skill)
    {}
    protected override void OnActiveExecute(ActiveSkillBase skill)
    {
        //debuffManager.DebuffDisplay(target, "EyesOfSnake", 20f); <- 타겟에게 표시할 디버프이미지 띄우는 함수
        //debuffManager.Debuff(target, "mark", 20f);
        //debuffManager.Debuff(target, "amplify", 50f);
        //debuffManager.Debuff(target, "slow", 70f);
        //해당 타겟에게 디버프 검. Debuff함수는 (GameObject target, str string, float debuffTime);
    }
    protected override void OnActiveExit(ActiveSkillBase skill)
    {}
    
    # endregion
}
