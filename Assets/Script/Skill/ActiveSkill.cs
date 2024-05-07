using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class ActiveSkill : Skill
{
    private Vector3 targetPoint;
    public GameObject skillIndicator;
    void Start()
    {
        data.checkCooltime = data.skillCooltime;
        data.checkUptime = data.skillUptime;
    }
    public override void UseSkill()
    {
        switch(data.skillNumber)
        {
            case 400: // 스킬 200번을 사용 시 타겟한 몬스터에 x의 데미지를 주는 공격 액티브 스킬이라고 가정
            Active_400();
            break;
            case 401: // 스킬 201번을 자기 공격력을 x초간 50% 증가시켜주는 버프 액티브 스킬이라고 가정
            Active_401();
            break;
            case 402: // 스킬 202번을 사용 시 x범위에 x데미지를 주는 공격 액티브 스킬이라고 가정
            Active_402();
            break;
        }
    }

    void Active_400()
    {
        // 액티브 스킬 버튼 눌렀을때 작동 코드 필요(버튼 눌렀을 때 타겟을 선택하세요 설명UI)

        StartCoroutine(WaitTargetSelected());
        if(target)
        {
            target.GetComponent<Monster>().HasAttacked(data.skillDamage);
        }
        StartCoroutine(WaitSkillCooltime());
    }

    void Active_401()
    {
        StartCoroutine(WaitSkillUptime());
        StartCoroutine(Buff_Damage_401(50, true));
    }

    void Active_402()
    {
        StartCoroutine(SelectRange(data.scopeRange, data.skillDamage));
    }
    IEnumerator Buff_Damage_401(float value, bool isPercent)
    {
        float buffValue;
        if(isPercent)
            buffValue = data.skillDamage * value / 100f;
        else
            buffValue = value;
        data.skillDamage += buffValue;
        while(data.checkUptime > 0)
        {
            yield return null;
        }
        data.skillDamage -= buffValue;
    }
    
    IEnumerator SelectRange(float skill_area, float damage)
    {
        SkillIndicator si = skillIndicator.GetComponent<SkillIndicator>();
        si.Set_Circle_Indicator(skill_area);
        while(true)
        {
            if(si.indicator == null)
            {
                targetPoint = si.targetPoint;
                Debug.Log(targetPoint + DateTime.Now.ToString());
                si.circleIndicator.SetActive(false);
                StartCoroutine(DealDamage_with_Area(skill_area, damage));
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator DealDamage_with_Area(float skill_area, float damage)
    {
        Collider2D[] cols_in_range = Physics2D.OverlapCircleAll(targetPoint, skill_area / 2);
        foreach (Collider2D col in cols_in_range)
        {
            if (col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission"))
            {
                col.GetComponent<Monster>().HasAttacked(damage);
            }
        }
        yield break;
    }
}
