using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : Skill
{
    void Start()
    {
        data.checkCooltime = data.skillCooltime;
        data.checkUptime = data.skillUptime;
    }
    public override void UseSkill()
    {

        switch(data.skillNumber)
        {

            case 100: // 스킬 100번을 투사체 있는 기본 단일 공격이라고 가정
                Vector3 user_pos = user.transform.position;
                Vector3 target_pos = target.transform.position;
                FireProjectile(user_pos, target_pos, false);
                break;
            case 101: // 스킬 101번을 투사체 없는 기본 단일 공격이라고 가정
                hitEffect = ResourceManager.Instance.Load<GameObject>("Prefabs/hit_effect_1") as GameObject;
                DealingOneTargetDamage();
                break;
            case 102: // 스킬 102번을 투사체 없는 기본 범위 공격이라고 가정
                DealingAOEDamage();
                break;
            case 103: // 펫 전용 테스트 평타 테스트 코드
                hitEffect = ResourceManager.Instance.Load<GameObject>("Prefabs/hit_effect_2") as GameObject;
                DealingOneTargetDamage();
                break;
        }
    }
}
