using UnityEngine;

public class MoveState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        Vector3 dir = (entity.TouchPos - entity.transform.position).normalized;
        entity.transform.position += dir * (entity.Data.MoveSpeed * Time.deltaTime);
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}