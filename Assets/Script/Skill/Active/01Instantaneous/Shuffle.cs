using UnityEngine;

public class Shuffle : InstantaneousSkill
{
    private enum CardType
    {
        CloverJack,
        HeartQueen,
        DiamondKing,
        Joker
    }
    
    private CardEffectBase _cardEffect;
    
    protected override void OnActiveEnter()
    {
        // (클로버 잭(Data{0}), 하트 퀸(Data{1}), 다이아몬드 킹(Data{2}), 조커: Data{3}) 중 하나를 랜덤하게 선택
        int random = Random.Range(0, 100);

        _cardEffect.Weapon = weapon;
        Data.CoolTime = _cardEffect.Data.CoolTime;
        
        _cardEffect.OnEnter();
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        INode.ENodeState result = _cardEffect.OnUpdate();
        
        if (result == INode.ENodeState.Success)
        {
            IsActive = false;
        }

        return result;
    }

    protected override void OnActiveExit()
    {
        _cardEffect.OnExit();
        _cardEffect = null;
    }
}