using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitCode
{
    TMPENEMY1,
    TMPENEMY2,
    TMPENEMY3,
    TMPENEMY4,
    TMPENEMY5,
    TMPENEMY6,
    TMPENEMY7

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