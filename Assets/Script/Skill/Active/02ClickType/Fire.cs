public class Fire : ClickTypeSkill
{
    protected override void OnActiveEnter()
    {
        if (weapon.owner.Target is null)
            return;
        
        if(weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            // Todo: 단일 공격
        }
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        ;
    }
}