using System.Linq;
using UnityEngine;

public class IdleState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        var colliders = RangeDetectionUtility.GetAttackTargets(entity.transform.position, Vector2.zero, entity.Data.CurrentWeapon.Data.attackRange);
        
        foreach (var collider in colliders.Where(collider => collider.CompareTag("Monster") || collider.CompareTag("Boss") || collider.CompareTag("Mission")))
        {
            var distanceFromEntityToCollider = Vector3.Distance(entity.transform.position, collider.transform.position);
            var distanceFromEntityToTarget = entity.Target == null ? 0.0f : Vector3.Distance(entity.transform.position, entity.Target.transform.position);
            
            // 가장 가까운 적을 타겟으로 설정
            if (entity.Target == null)
                entity.Target = collider.transform.gameObject;
            else if(distanceFromEntityToCollider < distanceFromEntityToTarget)
                entity.Target = collider.transform.gameObject;
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}