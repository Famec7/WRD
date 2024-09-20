using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Monster : MonoBehaviour, IPoolObject
{
    public float hp;
    public float speed;
    public float armor;
    public bool isMovable;
    public bool isDead = false;
    public Status status;
    
    public void HasAttacked(float damage)
    {
        hp -= damage;
        status.HP -= damage + damage * status.damageAmplification;
        if (status.HP <= 0 && !isDead)
            IsDead();
    }

    private void IsDead()
    {
        isDead = true;

        if (status.unitCode >= UnitCode.MISSIONBOSS1 && status.unitCode <= UnitCode.MISSIONBOSS6)
        {
            MissionManager.Instance.TargetMonsterList.Remove(this);
            RewardManager.instance.GetReward(status.unitCode);
        }

        MonsterHPBarPool.ReturnObject(transform.GetChild(1).GetComponent<MonsterHPBar>()); 
        MonsterPoolManager.Instance.ReturnObject(gameObject);
    }

    public void GetFromPool()
    {
        ;
    }

    public void ReturnToPool()
    {
        ;
    }
}
