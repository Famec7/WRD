using System;
using UnityEngine;

public abstract class AnimationBase : MonoBehaviour
{
    [SerializeField]
    protected AnimationCurve animationSpeed;

    public Transform Owner { get; set; }

    protected float endTime;
    
    public void SetOwnerAndEndTime(Transform owner, float endTime)
    {
        Owner = owner;
        
        SetTime(endTime);
    }
    
    public void SetTime(float time)
    {
        endTime = time;
        animationSpeed.keys[animationSpeed.length - 1].time = endTime;
    }

    public abstract void PlayAnimation();
    public abstract void StopAnimation();
}