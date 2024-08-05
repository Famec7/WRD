using System.Collections.Generic;

/// <summary>
/// 하위 노드에서 Failure이 나올 때까지 실행하는 노드
/// </summary>
public sealed class SequenceNode : INode
{
    private readonly List<INode> _childs;
    
    public SequenceNode(List<INode> childs)
    {
        _childs = childs;
    }
    
    public INode.ENodeState Evaluate()
    {
        if(_childs == null)
            return INode.ENodeState.Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;
                case INode.ENodeState.Failure:
                    return INode.ENodeState.Failure;
                default:
                    break;
            }
        }
        
        return INode.ENodeState.Success;
    }
}