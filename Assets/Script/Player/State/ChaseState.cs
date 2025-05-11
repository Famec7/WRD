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
            entity.ChangeState(PlayerController.State.Attack);
            return;
        }
        
        // 타겟이 범위 밖에 있으면 이동
        Vector3 targetPos = entity.Target.transform.position;
        Vector3 dir = (targetPos - entity.transform.position).normalized;
        entity.MoveDir = dir;
        
        Vector3 newPosition = entity.transform.position + dir * (entity.Data.MoveSpeed * Time.deltaTime);
        
        newPosition.x = Mathf.Clamp(newPosition.x, entity.MinX, entity.MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, entity.MinY, entity.MaxY);
        
        entity.transform.position = newPosition;
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}