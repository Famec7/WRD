using System;
using UnityEngine.Events;

public sealed class ActionNode : INode
{
    private readonly Func<INode.ENodeState> _onUpdate;
    
    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.ENodeState Evaluate() => _onUpdate?.Invoke() ?? INode.ENodeState.Failure;
}