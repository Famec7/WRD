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

    public float resist;
    public float defense;

    public string monsterName;

    #region StatusEffect

    [HideInInspector]
    public bool IsWound; // 자상
    
    [HideInInspector]
    public bool IsMark; // 표식
    
    [HideInInspector]
    public float damageAmplification; // 데미지 증폭
    
    [HideInInspector]
    public bool IsElectricShock; // 감전

    #endregion
    
    
    public void SetUnitStatus(UnitCode unitCode)
    {
        int wave = GameManager.instance.wave-1;
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

        maxHP = HP;
        resist = MonsterDataManager.instance.resistData[wave];
        monsterName = MonsterDataManager.instance.monsterNameData[wave];
        moveSpeed = MonsterDataManager.instance.speedData[wave];
        IsWound = false;
        IsMark = false;
        damageAmplification = 0;
        IsElectricShock = false;
    }

    public void SetMissionUnitStatus(UnitCode unitCode)
    {
        int idx = unitCode - UnitCode.MISSIONBOSS1;
        unitCode = MissionMonsterManager.instance.unitCodeData[idx];       
        HP = MissionMonsterManager.instance.HPData[idx];
        defense = MissionMonsterManager.instance.defenseData[idx];
        maxHP = HP;
        resist = MissionMonsterManager.instance.resistData[idx];
        monsterName = MissionMonsterManager.instance.monsterNameData[idx];
        moveSpeed = MissionMonsterManager.instance.speedData[idx];
        IsWound = false;
        IsMark = false;
    }
}
