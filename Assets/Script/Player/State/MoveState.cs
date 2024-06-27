using UnityEngine;

public class MoveState : ControlState
{
    public override void Enter(PlayerController entity)
    {
        ;
    }

    public override void Execute(PlayerController entity)
    {
        Vector3 dir = (entity.TouchPos - entity.transform.position).normalized;
        entity.transform.position += dir * (entity.MoveSpeed * Time.deltaTime);
    }

    public override void Exit(PlayerController entity)
    {
        ;
    }
}