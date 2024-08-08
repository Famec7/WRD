using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Status : MonoBehaviour
{
    public UnitCode unitCode;
    public float maxHP;
    public float HP;
    public float moveSpeed;

    public float resist;
    public float defense;

    public string monsterName;
    
    [HideInInspector]
    public bool woundStack;
    [HideInInspector]
    public bool markStack;
    
    public void SetUnitStatus(UnitCode unitCode)
    {
        int wave = GameManager.instance.wave-1;
        unitCode = MonsterDataManager.instance.unitCodeData[wave];

        if (unitCode <= UnitCode.SPROUN)
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
        woundStack = false;
        markStack = false;
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
        woundStack = false;
        markStack = false;
    }
}
