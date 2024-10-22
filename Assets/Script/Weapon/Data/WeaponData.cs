using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData 
{
    public int ID { get; set; }

    public string WeaponName { get; set; }
    public int Type1 { get; }
    public float AttackDamage { get; set; }
    public int skillA;
    public int skillP;
    public int rType;
    public int reload;
    public int reloadS;
    public int num { get; set; }

    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float attackAoe;
    public float dps;

    public string WeaponClass { get; set; }
    public string Combi { get; set; }
    public string MainCombi { get; set; }
    public string description;
    public WeaponTier tier;
}
