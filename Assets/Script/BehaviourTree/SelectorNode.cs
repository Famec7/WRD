using System.Collections.Generic;

/// <summary>
/// 하위 노드 중 하나만 선택하는 노드
/// </summary>
public sealed class SelectorNode : INode
{
    private readonly List<INode> _childs;
    
    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }
    
    public INode.ENodeState Evaluate()
    {
        if (_childs == null)
            return INode.ENodeState.Failure;

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;
                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;
            }
        }
        
        return INode.ENodeState.Failure;
    }
}