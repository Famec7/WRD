using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RangeDetectionUtility
{
    private static int _colliderSize = 100;
    
    /// <summary>
    /// 무기 공격 범위 내의 적 탐지 (원, 부채꼴 모양)
    /// <param name="position"> 탐지 범위 중심 </param>
    /// <param name="direction"> 탐지 범위 방향 </param>
    /// <param name="radius"> 반지름 </param>
    /// <param name="degree"> 각도 </param>
    /// <param name="layerMask"> 탐지할 레이어 </param>
    /// <returns> 탐지된 적 몬스터 리스트 </returns>
    /// </summary>
    public static List<Collider2D> GetAttackTargets(Vector2 position, Vector2 direction, float radius,
        float degree = 360f, LayerMask layerMask = default)
    {
        var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
        
        if (Math.Abs(degree - 360f) < float.Epsilon)
            return colliders.ToList();
        
        List<Collider2D> targets = new List<Collider2D>();
        foreach (var col in colliders)
        {
            Vector3 targetDirection = col.transform.position - (Vector3)position;
            float angle = Vector3.Angle(direction, targetDirection);
            if (angle <= degree / 2)
            {
                targets.Add(col);
#if DETECTION_DRAW
                Debug.DrawLine(position, col.transform.position, Color.red);
#endif
            }
        }

        return targets;
    }

    /// <summary>
    ///  무기 공격 범위 내의 적 탐지 (사각형 모양)
    /// </summary>
    /// <param name="position"> 탐지 범위 중심 </param>
    /// <param name="size"> 가로 세로 사이즈 (벡터)</param>
    /// <param name="direction"> 탐지 범위 방향(일자로) </param>
    /// <param name="layerMask"> 탐지할 레이어 </param>
    /// <returns> 탐지된 적 몬스터 리스트 </returns>
    /// </summary>
    public static List<Collider2D> GetAttackTargets(Vector2 position, Vector2 size, Vector2 direction = default,
        LayerMask layerMask = default)
    {
        var colliders = new Collider2D[_colliderSize];
        
        if (direction == default)
        {
            colliders = Physics2D.OverlapBoxAll(position, size, 0, layerMask);
            return colliders.ToList();
        }
        else
        {
            Vector2 pos = position + direction.normalized * size / 2;
            colliders = Physics2D.OverlapBoxAll(pos, size, Vector2.SignedAngle(Vector2.right, direction), layerMask);
            
#if DETECTION_DRAW
            foreach (var col in colliders)
            {
                Debug.DrawLine(position, col.transform.position, Color.red);
            }
#endif
            return colliders.ToList();
        }
    }
}