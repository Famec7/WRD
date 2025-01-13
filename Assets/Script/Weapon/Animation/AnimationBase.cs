using System;
using UnityEngine;

public abstract class AnimationBase : MonoBehaviour
{
    [Header("애니메이션 속도 그래프")]
    [SerializeField]
    protected AnimationCurve animationSpeed;
    
    [Header("애니메이션 속도")]
    [SerializeField]
    private float speed = 1.0f;

    /// <summary>
    /// 애니메이션 종료 시간
    /// </summary>
    protected float endTime;
    
    public void SetTime(float time)
    {
        endTime = time;
        
        Keyframe[] keys = animationSpeed.keys;
        keys[keys.Length - 1].time = time;
        animationSpeed.keys = keys;
    }

    /// <summary>
    /// 애니메이션을 실행하는 함수
    /// </summary>
    public abstract void PlayAnimation();
    
    /// <summary>
    /// 애니메이션을 중지하는 함수
    /// </summary>
    public abstract void StopAnimation();
    
    protected float CalculateElapsedTime(ref float elapsedTime)
    {
        elapsedTime += Time.deltaTime * speed;
        float t = elapsedTime / endTime;
        
        return animationSpeed.Evaluate(t);
    }
}