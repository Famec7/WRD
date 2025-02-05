using System.Linq;
using UnityEngine;

public class IdleState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        entity.Target = null;
    }

    public void Execute(PlayerController entity)
    {
        if (entity.Data.CurrentWeapon is not null)
        {
            entity.Target = entity.FindNearestTarget();

            if (entity.IsTouchTarget is false)
            {
                entity.ChangeState(PlayerController.State.Attack);
            }
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}