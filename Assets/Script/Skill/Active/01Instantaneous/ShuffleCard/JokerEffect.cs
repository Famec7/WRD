using System.Collections.Generic;

public class JokerEffect : CardEffectBase
{
    private readonly List<CardEffectBase> _cardEffects;
    private List<bool> _isComplete;
    
    public JokerEffect(WeaponBase weapon) : base(weapon)
    {
        _cardEffects = new List<CardEffectBase>
        {
            new CloverJackEffect(Weapon),
            new HeartQueenEffect(Weapon),
            new DiamondKingEffect(Weapon)
        };
        
        _isComplete = new List<bool>(_cardEffects.Count) {false, false, false};
    }

    public override void OnEnter()
    {
        foreach (var cardEffect in _cardEffects)
        {
            cardEffect.OnEnter();
        }
    }

    public override INode.ENodeState OnUpdate()
    {
        bool isAllComplete = true;
        
        for (int i = 0; i < _cardEffects.Count; i++)
        {
            if (!_isComplete[i])
            {
                INode.ENodeState result = _cardEffects[i].OnUpdate();
                if (result == INode.ENodeState.Success)
                {
                    _cardEffects[i].OnExit();
                    _isComplete[i] = true;
                }
                else
                {
                    isAllComplete = false;
                }
            }
        }
        
        return isAllComplete ? INode.ENodeState.Success : INode.ENodeState.Running;
    }

    public override void OnExit()
    {
        foreach (var cardEffect in _cardEffects)
        {
            if (!_isComplete[_cardEffects.IndexOf(cardEffect)])
            {
                cardEffect.OnExit();
            }
        }
    }
}