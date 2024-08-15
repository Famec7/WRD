public interface INode
{
    public enum ENodeState
    {
        Running,
        Success,
        Failure
    }
    
    public ENodeState Evaluate();
}