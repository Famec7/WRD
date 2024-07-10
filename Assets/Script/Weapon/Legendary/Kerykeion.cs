using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kerykeion : RangedWeapon
{
    public SkillButton fohnBtn;
    public SkillButton eyesOfSnakeBtn;
    private GameObject skillBtn1;
    private GameObject skillBtn2;
    /*
    void OnEnable()
    {
        if (owner is PlayerController)
        {
            GameObject btnParent = GameObject.Find("SkillButtonParent");
            skillBtn1 = btnParent.transform.GetChild(0).gameObject;
            skillBtn2 = btnParent.transform.GetChild(1).gameObject;
            skillBtn1.SetActive(true);
            skillBtn2.SetActive(true);
            fohnBtn = skillBtn1.GetComponent<SkillButton>();
            fohnBtn.activeSkill = GetComponent<Fohn>();
            eyesOfSnakeBtn = skillBtn2.GetComponent<SkillButton>();
            eyesOfSnakeBtn.activeSkill = GetComponent<EyesOfSnake>();
        }
    }
    void OnDisable()
    {
        if (owner is PlayerController)
        {
            fohnBtn.GetComponent<SkillButton>().activeSkill = null;
            fohnBtn = null;
            eyesOfSnakeBtn.GetComponent<SkillButton>().activeSkill = null;
            eyesOfSnakeBtn = null;
            skillBtn1.SetActive(false);
            skillBtn2.SetActive(false);
        }
    }
    */
}
