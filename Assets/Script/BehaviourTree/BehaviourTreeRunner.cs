public sealed class BehaviourTreeRunner
{
    private readonly INode _rootNode;
    
    public BehaviourTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }
    
    public void Operator()
    {
        _rootNode.Evaluate();
    }
}