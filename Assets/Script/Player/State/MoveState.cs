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