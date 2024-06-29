using UnityEngine;

public class ChaseState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        if (Vector3.Distance(entity.transform.position, entity.Target.transform.position) > entity.Data.CurrentWeapon.Data.attackRange)
        {
            Vector3 targetPos = entity.Target.transform.position;
            Vector3 dir = (targetPos - entity.transform.position).normalized;
            entity.transform.position += dir * (entity.Data.MoveSpeed * Time.deltaTime * 2);
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}