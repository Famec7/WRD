using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PassiveSkill : Skill
{
    public override void UseSkill()
    {
        switch(data.skillNumber)
        {
            case 200: // 스킬 200번을 투사체가 없는 공격 시 x%확률로 x의 단일 타겟 데미지를 주는 패시브 스킬이라고 가정
            Passive_200();
            break;
            case 201: // 스킬 201번을 투사체가 없는 공격 시 x%확률로 x의 범위 전타겟 데미지를 주는 패시브 스킬이라고 가정
            Passive_201();
            break;
        }
    }
    
    void Passive_200()
    {
        float random_seed = UnityEngine.Random.Range(0f, 1f);
        if(random_seed < data.skillChance)
        {
            //이펙트 출력 코드 필요
            target.GetComponent<Monster>().HasAttacked(data.skillDamage);
        }
    }


    void Passive_201()
    {
        float random_seed = UnityEngine.Random.Range(0f, 1f);
        if(random_seed < data.skillChance)
        {
            //이펙트 출력 코드 필요

            Vector3 target_pos = target.transform.position;
            Collider2D[] cols_in_range = Physics2D.OverlapCircleAll(target_pos, data.scopeRange);
            foreach(Collider2D col in cols_in_range)
            {
                if(col.CompareTag("Monster") || col.CompareTag("Boss") || col.CompareTag("Mission"))
                {
                    col.GetComponent<Monster>().HasAttacked(data.skillDamage);
                }
            }
        }
    }
}
