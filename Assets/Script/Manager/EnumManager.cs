using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCode
{
    SLIME,
    WOLF,
    SKELETON,
    GHOST,
    GOLEM,
    SPROUN,
    BOSS1,
    BOSS2,
    BOSS3,
    BOSS4,
    BOSS5,
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
    UNNORMAL,
    RARE,
    EPIC,
    LEGENDARY,
    COUNT,
    MYTH
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
}