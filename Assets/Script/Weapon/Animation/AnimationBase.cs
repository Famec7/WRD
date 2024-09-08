using System;
using UnityEngine;

public abstract class AnimationBase : MonoBehaviour
{
    [Header("애니메이션 속도 그래프")]
    [SerializeField]
    protected AnimationCurve animationSpeed;

    /// <summary>
    /// 애니메이션이 실행되는 Transform
    /// </summary>
    public Transform Owner { get; set; }

    /// <summary>
    /// 애니메이션 종료 시간
    /// </summary>
    protected float endTime;
    
    public void SetTime(float time)
    {
        endTime = time;
        animationSpeed.keys[animationSpeed.length - 1].time = endTime;
    }

    /// <summary>
    /// 애니메이션을 실행하는 함수
    /// </summary>
    public abstract void PlayAnimation();
    
    /// <summary>
    /// 애니메이션을 중지하는 함수
    /// </summary>
    public abstract void StopAnimation();
}