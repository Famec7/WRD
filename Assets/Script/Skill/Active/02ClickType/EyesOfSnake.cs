public class EyesOfSnake : ClickTypeSkill
{
    protected override void OnActiveEnter()
    {
        FindTarget();

        if (target.TryGetComponent(out Monster monster))
        {
            float duration = Data.GetValue(0);
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