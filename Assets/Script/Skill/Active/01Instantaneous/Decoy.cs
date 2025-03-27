using System.Collections.Generic;
using UnityEngine;

public class Decoy : InstantaneousSkill
{
    [SerializeField] private List<CloneController> _clones;
    [SerializeField] private float _randomRange = 1.0f;
    
    [SerializeField]
    private bool _isMelee = false;

    protected override void Init()
    {
        base.Init();

        PassiveSkillData passiveSkillData = weapon.GetPassiveSkill(0).Data;
        foreach (var clone in _clones)
        {
            clone.transform.parent = null;
            clone.Despawn();
            if (clone.Data.CurrentWeapon is Yeouibong yeouibong)
            {
                yeouibong.SetData(passiveSkillData, Data, _isMelee);
            }
        }
    }

    public override void OnActiveEnter()
    {
        Vector2 monsterTargetPos = GetMonsterTargetPosition();
        
        foreach (var clone in _clones)
        {
            Vector2 spawnPosition = GetRandomPosition(monsterTargetPos);
            
            /*clone.SetFlip(spawnPosition.x < monsterTargetPos.x);*/
            clone.Spawn(spawnPosition);
        }
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
    
    private Vector2 GetMonsterTargetPosition()
    {
        if (weapon.owner.Target == null)
        {
            GameObject target = weapon.owner.FindNearestTarget();
            
            if (target == null)
            {
                return weapon.owner.transform.position;
            }
            
            return target.transform.position;
        }
        else
        {
            return weapon.owner.Target.transform.position;
        }
    }
    
    private Vector2 GetRandomPosition(Vector2 targetPosition)
    {
        Vector2 randomPosition = targetPosition;
        randomPosition.x += Random.Range(-_randomRange, _randomRange);
        randomPosition.y += Random.Range(-_randomRange, _randomRange);
        return randomPosition;
    }
}