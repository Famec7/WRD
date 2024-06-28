using UnityEngine;

public class ChaseState : ControlState
{
    public override void Enter(PlayerController entity)
    {
        ;
    }

    public override void Execute(PlayerController entity)
    {
        if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) > entity.CurrentWeapon.Data.attackRange)
        {
            Vector3 targetPos = entity.Target.transform.position;
            Vector3 dir = (targetPos - entity.transform.position).normalized;
            entity.transform.position += dir * (entity.MoveSpeed * Time.deltaTime * 2);
        }
    }

    public override void Exit(PlayerController entity)
    {
        ;
    }
}