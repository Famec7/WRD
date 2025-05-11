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
    }

    public void Execute(PlayerController entity)
    {
        if (entity.IsTargetNullOrInactive())
        {
            entity.ChangeState(PlayerController.State.IDLE);
            return;
        }

        CheckFlip(entity);
        bool isAttackSuccess = entity.Data.CurrentWeapon.UpdateAttack();

        if (isAttackSuccess is false)
        {
            return;
        }

        bool isTargetInRange = entity.IsTargetInRange();
        if (entity.IsTouchTarget && isTargetInRange == false)
        {
            entity.ChangeState(PlayerController.State.CHASE);
            return;
        }
        if (entity.IsTouchTarget == false)
        {
            entity.ChangeState(PlayerController.State.IDLE);
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
    
    private void CheckFlip(PlayerController entity)
    {
        float targetPosX = entity.Target.transform.position.x;
        float playerPosX = entity.transform.position.x;

        bool isRight = targetPosX > playerPosX;
        entity.SetFlip(isRight);
    }
}