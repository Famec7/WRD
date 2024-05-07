using System;
using UnityEngine;

public abstract class StatusEffect
{
    protected float duration;
    protected WaitForSeconds waitTime;
    protected GameObject target;
    
    public GameObject Target { get; }

    public float Duration => duration;
    
    public StatusEffect(GameObject target, float duration = 0f)
    {
        SetEffect(duration, target);
    }

    public void SetEffect(float duration, GameObject target)
    {
        this.duration = duration;
        waitTime = new WaitForSeconds(duration);
        this.target = target;
    }

    public abstract void ApplyEffect();

    public abstract void RemoveEffect();
}