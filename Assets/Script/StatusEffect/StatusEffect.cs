using System;
using UnityEngine;

public abstract class StatusEffect
{
    protected float duration;
    protected WaitForSeconds waitTime;
    protected GameObject target;

    protected MonsterEffecter monsterEffecter;

    public float Duration => duration;
    
    public StatusEffect(GameObject target, float duration = 0f)
    {
        SetEffect(duration, target);

        FindMonsterEffecter();
    }

    private void SetEffect(float duration, GameObject target)
    {
        this.duration = duration;
        waitTime = new WaitForSeconds(duration);
        this.target = target;
    }

    /// <summary>
    /// 상태이상을 적용하는 함수
    /// </summary>
    public abstract void ApplyEffect();

    /// <summary>
    /// 상태이상을 제거하는 함수
    /// </summary>
    public abstract void RemoveEffect();
    
    /// <summary>
    /// 기존 상태이상에서 중첩되는 상태이상을 합치는 함수
    /// </summary>
    /// <param name="statusEffect"> 중첩할 상태이상 </param>
    public virtual void CombineEffect(StatusEffect statusEffect)
    {
        AddDuration(statusEffect.Duration);
    }
    
    /// <summary>
    /// 상태이상의 지속시간을 추가하는 함수
    /// </summary>
    /// <param name="duration"> 추가할 지속시간 </param>
    private void AddDuration(float duration)
    {
        this.duration += duration;
        waitTime = new WaitForSeconds(this.duration);
    }

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