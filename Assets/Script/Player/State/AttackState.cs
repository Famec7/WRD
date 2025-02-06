public class AttackState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        if (entity.Data.CurrentWeapon.enabled is false)
        {
            entity.ChangeState(PlayerController.State.IDLE);
            return;
        }

        if (entity.Target is null)
        {
            entity.ChangeState(PlayerController.State.IDLE);
            return;
        }
        
        float targetPosX = entity.Target.transform.position.x;
        float playerPosX = entity.transform.position.x;

        bool isRight = targetPosX > playerPosX;
        entity.SetFlip(isRight);
    }

    public void Execute(PlayerController entity)
    {
        if (entity.IsTargetNullOrInactive())
        {
            entity.ChangeState(PlayerController.State.IDLE);
            return;
        }

        bool isAttackSuccess = entity.Data.CurrentWeapon.UpdateAttack();

        if (isAttackSuccess is false)
        {
            return;
        }

        if (entity.IsTouchTarget && entity.IsTargetInRange() is false)
        {
            entity.ChangeState(PlayerController.State.CHASE);
        }
        else
        {
            entity.ChangeState(PlayerController.State.IDLE);
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}