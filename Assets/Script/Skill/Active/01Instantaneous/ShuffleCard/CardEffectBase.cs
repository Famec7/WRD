using System;

public abstract class CardEffectBase
{
    public ActiveSkillData Data { get; set; }

    protected CardEffectBase(WeaponBase weapon)
    {
        Weapon = weapon;
    }
    
    public WeaponBase Weapon { get; set; }
    
    public abstract void OnEnter();
    public abstract bool OnUpdate();
    public abstract void OnExit();
}