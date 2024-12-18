using System;
using UnityEngine;

public abstract class StatusEffect
{
    protected float duration;
    protected WaitForSeconds waitTime;
    protected GameObject target;

    protected MonsterEffecter monsterEffecter;
    
    public GameObject Target { get; }

    public float Duration => duration;
    
    public StatusEffect(GameObject target, float duration = 0f)
    {
        SetEffect(duration, target);

        FindMonsterEffecter();
    }

    public void SetEffect(float duration, GameObject target)
    {
        this.duration = duration;
        waitTime = new WaitForSeconds(duration);
        this.target = target;
    }

    public abstract void ApplyEffect();

    public abstract void RemoveEffect();

    /// <summary>
    /// 몬스터 오브젝트에 있는 MonsterEffecter 컴포넌트 찾아주는 함수(스파게티가 만들어지는 첫 시작점)
    /// </summary>
    /// <returns></returns>
    MonsterEffecter FindMonsterEffecter()
    {
        MonsterEffecter monsterEffecter = target.GetComponentInChildren<MonsterEffecter>();

        //if (monsterEffecter == null) Debug.LogWarning("적에게 적 이펙터가 존재하지 않음");
        //else Debug.Log("적에게 이펙터가 존재함");

        this.monsterEffecter = monsterEffecter;

        return monsterEffecter;
    }
}