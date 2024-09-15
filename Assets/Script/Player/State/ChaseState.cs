using UnityEngine;

public class ChaseState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        if (entity.IsTargetInRange())
        {
            entity.ChangeState(PlayerController.State.IDLE);
            return;
        }
        
        // 타겟이 범위 밖에 있으면 이동
        Vector3 targetPos = entity.Target.transform.position;
        Vector3 dir = (targetPos - entity.transform.position).normalized;
        entity.MoveDir = dir;
        
        entity.transform.position += dir * (entity.Data.MoveSpeed * Time.deltaTime * 2);
    }

    public void Exit(PlayerController entity)
    {
        entity.Target = null;
    }
}