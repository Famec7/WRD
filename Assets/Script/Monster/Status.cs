using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Status : MonoBehaviour
{   
    public UnitCode unitCode { get; }
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
        int wave = GameManager.instance.wave;

        unitCode = MonsterDataManager.instance.unitCodeData[wave];
        HP = MonsterDataManager.instance.HPData[wave];
        maxHP = HP;
        resist = MonsterDataManager.instance.resistData[wave];
        defense = MonsterDataManager.instance.defenseData[wave];
        monsterName = MonsterDataManager.instance.monsterNameData[wave];
        moveSpeed = MonsterDataManager.instance.speedData[wave];
        woundStack = false;
        markStack = false;
    }
}
