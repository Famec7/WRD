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
        entity.FindNearestTarget();
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}