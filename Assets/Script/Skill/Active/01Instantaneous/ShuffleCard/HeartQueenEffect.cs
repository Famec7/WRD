﻿using UnityEngine;

public class HeartQueenEffect : CardEffectBase
{
    private float _time;
    private Vector3 _range = new Vector3(3, 5, 0);
    
    public HeartQueenEffect(WeaponBase weapon) : base(weapon)
    {
        Data = SkillManager.Instance.GetActiveSkillData("shuffle - heart queen");
        _time = Data.GetValue(0);
    }

    public override void OnEnter()
    {
        // 아군 무기들의 공격속도가 10초 동안 100% 빨라진다. 또한 확률로 발동하는 스킬들의 확률이 20%p 상승한다.

        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            WeaponBase weapon = character.Data.CurrentWeapon;
            
            if (weapon == null)
            {
                continue;
            }
            
            ApplyBuffToCharacter(character);
        }

        WeaponManager.Instance.OnWeaponEquipped += ApplyBuffToCharacter;
        WeaponManager.Instance.OnWeaponDetached += RemoveBuffFromCharacter;
    }

    public override INode.ENodeState OnUpdate()
    {
        if (_time <= 0)
        {
            return INode.ENodeState.Success;
        }
        
        _time -= Time.deltaTime;
        return INode.ENodeState.Running;
    }

    public override void OnExit()
    {
        for (int i = 0; i < (int)CharacterManager.CharacterType.Count; i++)
        {
            CharacterController character = CharacterManager.Instance.GetCharacter(i);
            WeaponBase weapon = character.Data.CurrentWeapon;
            
            if (weapon == null)
            {
                continue;
            }
            
            RemoveBuffFromCharacter(character);
        }
        
        WeaponManager.Instance.OnWeaponEquipped -= ApplyBuffToCharacter;
        WeaponManager.Instance.OnWeaponDetached -= RemoveBuffFromCharacter;
    }
    
    private void ApplyBuffToCharacter(CharacterController character)
    {
        WeaponBase weapon = character.Data.CurrentWeapon;
        
        float originalAttackSpeed = character.Data.CurrentWeapon.Data.AttackSpeed;
        weapon.SetAttackDelay(originalAttackSpeed + originalAttackSpeed * Data.GetValue(1));

        weapon.passiveSkill.Data.Chance += (int)Data.GetValue(2);
    }
    
    private void RemoveBuffFromCharacter(CharacterController character)
    {
        character.Data.CurrentWeapon.SetAttackDelay(character.Data.CurrentWeapon.Data.AttackSpeed);
        character.Data.CurrentWeapon.passiveSkill.Data.Chance -= (int)Data.GetValue(2);
    }
}