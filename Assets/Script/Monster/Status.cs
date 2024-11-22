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
    public float moveSpeed;
    
    public float originalSpeed;

    public float resist;
    public float defense;

    public string monsterName;

    #region StatusEffect

    [HideInInspector]
    public bool IsWound; // 자상
    
    [HideInInspector]
    public bool IsMark; // 표식
    
    [HideInInspector]
    public float DamageAmplification; // 데미지 증폭
    
    [HideInInspector]
    public bool IsElectricShock; // 감전
    
    [HideInInspector]
    public bool IsJokerMark; // 조커 표식
    
    [HideInInspector]
    public float DevilBulletDamageAmplification; // 악탄
    
    [HideInInspector]
    public bool PreventWoundConsumption; // 자상 소모 방지

    #endregion
    
    
    public void SetUnitStatus(UnitCode unitCode)
    {
        int wave = GameManager.Instance.wave-1;
        unitCode = MonsterDataManager.instance.unitCodeData[wave];

        if (unitCode < UnitCode.ELITEMONSTER1)
        {
            HP = MonsterDataManager.instance.HPData[wave];
            defense = MonsterDataManager.instance.defenseData[wave];
        }

        else
        {
            HP = MonsterDataManager.instance.bossHPData[wave];
            defense = MonsterDataManager.instance.bossDefenseData[wave];
        }
        
        resist = MonsterDataManager.instance.resistData[wave];
        monsterName = MonsterDataManager.instance.monsterNameData[wave];
        moveSpeed = MonsterDataManager.instance.speedData[wave];
        originalSpeed = moveSpeed;
        
        maxHP = HP;
        ResetStatus();
    }

    public void SetMissionUnitStatus(UnitCode unitCode)
    {
        int idx = unitCode - UnitCode.MISSIONBOSS1;
        unitCode = MissionMonsterManager.instance.unitCodeData[idx];       
        HP = MissionMonsterManager.instance.HPData[idx];
        defense = MissionMonsterManager.instance.defenseData[idx];
        resist = MissionMonsterManager.instance.resistData[idx];
        monsterName = MissionMonsterManager.instance.monsterNameData[idx];
        moveSpeed = MissionMonsterManager.instance.speedData[idx];
        originalSpeed = moveSpeed;

        maxHP = HP;
        ResetStatus();
    }
    
    public void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }
    
    public void ResetStatus()
    {
        IsWound = false;
        IsMark = false;
        DamageAmplification = 0;
        IsElectricShock = false;
        IsJokerMark = false;
        DevilBulletDamageAmplification = 0;
    }
}
