using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Serialization;

public class Status : MonoBehaviour
{
    public UnitCode unitCode;
    public float maxHP;
    public float HP;

    // 이속감소의 최대 감속률
    private const float _maxSlowDownRate = 0.3f;
    private float _moveSpeed;
    public float MoveSpeed
    {
        get
        {
            if (moveSpeedMultiplier < _maxSlowDownRate)
            {
                return _moveSpeed * _maxSlowDownRate;
            }
            
            return _moveSpeed * moveSpeedMultiplier;
        }
        set => _moveSpeed = value;
    }

    public float originalSpeed;

    public float resist;

    public string monsterName;

    #region StatusEffect

    [HideInInspector]
    public int WoundStack; // 자상
    
    [HideInInspector]
    public int MarkStack; // 표식
    
    [HideInInspector]
    public float DamageAmplification; // 데미지 증폭
    
    [HideInInspector]
    public int ElectricShockStack; // 감전
    
    [HideInInspector]
    public int IsJokerMark; // 조커 표식
    
    [HideInInspector]
    public float DevilBulletDamageAmplification; // 악탄
    
    [HideInInspector]
    public bool PreventWoundConsumption; // 자상 소모 방지
    
    [HideInInspector]
    public float moveSpeedMultiplier = 1f; // 이동속도 배수
    
    [HideInInspector]
    public int stunStack; // 스턴

    #endregion
    
    
    public void SetUnitStatus(UnitCode unitCode)
    {
        int wave = GameManager.Instance.wave-1;
        unitCode = MonsterDataManager.instance.unitCodeData[wave];

        if (unitCode < UnitCode.ELITEMONSTER1)
        {
            HP = MonsterDataManager.instance.HPData[wave];
        }
        else
        {
            HP = MonsterDataManager.instance.bossHPData[wave];
        }
        
        resist = MonsterDataManager.instance.resistData[wave];
        monsterName = MonsterDataManager.instance.monsterNameData[wave];
        MoveSpeed = MonsterDataManager.instance.speedData[wave];
        originalSpeed = MoveSpeed;
        
        maxHP = HP;
        ResetStatus();
    }

    public void SetMissionUnitStatus(UnitCode unitCode)
    {
        int idx = unitCode - UnitCode.MISSIONBOSS1;
        unitCode = MissionMonsterManager.instance.unitCodeData[idx];       
        HP = MissionMonsterManager.instance.HPData[idx];
        //resist = MissionMonsterManager.instance.resistData[idx];
        monsterName = MissionMonsterManager.instance.monsterNameData[idx];
        MoveSpeed = MissionMonsterManager.instance.speedData[idx];
        originalSpeed = MoveSpeed;

        maxHP = HP;
        ResetStatus();
    }
    
    public void ResetSpeed()
    {
        MoveSpeed = originalSpeed;
    }
    
    public void ResetStatus()
    {
        WoundStack = 0;
        MarkStack = 0;
        DamageAmplification = 0;
        ElectricShockStack = 0;
        IsJokerMark = 0;
        DevilBulletDamageAmplification = 0;
        stunStack = 0;
        moveSpeedMultiplier = 1f;
        
        StatusEffectManager.Instance.RemoveAllStatusEffects(this);
    }
}
