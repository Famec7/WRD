using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCode
{
    MONSTER1,
    MONSTER2,
    MONSTER3,
    MONSTER4,
    MONSTER5,
    MONSTER6,
    ELITEMONSTER1,
    ELITEMONSTER2,
    ELITEMONSTER3,
    ELITEMONSTER4,
    ELITEMONSTER5,
    BOSS1,
    BOSS2,
    BOSS3,
    BOSS4,
    BOSS5,
    BOSS6,
    MISSIONBOSS1, 
    MISSIONBOSS2,
    MISSIONBOSS3,
    MISSIONBOSS4,
    MISSIONBOSS5,
    MISSIONBOSS6,
}



public enum SkillType
{
    EMPTY,
    BASIC,
    PASSIVE,
    AURA,
    ACTIVE,
    COUNT
}

public enum WeaponTier
{
    Empty,
    Element,
    UNNORMAL,
    RARE,
    EPIC,
    LEGENDARY,
    MYTH,
    COUNT
}

public enum PopUpUIType
{
    COMBINE,
    DESCRIPTION,
    MISSION,
}

public enum UIType
{
    INVENTORY,
    COMBINE,
    MissionDescription,
    RECENTEARN,
    NORMALINVENTORY,
    ALLSHOWINVENTORY,
    COUNT,
    MissionCheck
}