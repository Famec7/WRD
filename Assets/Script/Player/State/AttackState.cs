public class AttackState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        bool isAttackSuccess = entity.Data.CurrentWeapon.UpdateAttack();
        
        if (!isAttackSuccess)
        {
            entity.ChangeState(PlayerController.State.CHASE);
        }
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}