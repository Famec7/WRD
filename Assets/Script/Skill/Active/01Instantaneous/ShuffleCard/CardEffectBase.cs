using System;
using UnityEngine;

public abstract class CardEffectBase
{
    public ActiveSkillData Data { get; set; }
    public AudioClip Sfx { get; set; }

    protected CardEffectBase(WeaponBase weapon, AudioClip sfx = null)
    {
        Weapon = weapon;
        Sfx = sfx;
    }
    
    public WeaponBase Weapon { get; set; }
    
    public abstract void OnEnter();
    public abstract bool OnUpdate();
    public abstract void OnExit();
}