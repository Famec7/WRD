using System.Collections.Generic;

public class JokerEffect : CardEffectBase
{
    private List<CardEffectBase> _cardEffects;
    
    public JokerEffect(WeaponBase weapon) : base(weapon)
    {
    }

    public override void OnEnter()
    {
        _cardEffects = new List<CardEffectBase>
        {
            new CloverJackEffect(Weapon),
            new HeartQueenEffect(Weapon),
            new DiamondKingEffect(Weapon)
        };
    }

    public override INode.ENodeState OnUpdate()
    {
        foreach (var cardEffect in _cardEffects)
        {
            cardEffect.OnUpdate();
        }
        
        return INode.ENodeState.Running;
    }

    public override void OnExit()
    {
        foreach (var cardEffect in _cardEffects)
        {
            cardEffect.OnExit();
        }
    }
}