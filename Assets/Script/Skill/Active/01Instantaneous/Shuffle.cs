using System;
using Random = UnityEngine.Random;

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
        float random = Random.Range(0, 100);

        float cloverJack = Data.GetValue(0);
        float heartQueen = Data.GetValue(1);
        float diamondKing = Data.GetValue(2);
        float joker = Data.GetValue(3);

        _cardEffect = random switch
        {
            _ when random < cloverJack => new CloverJackEffect(weapon),
            _ when random < heartQueen => new HeartQueenEffect(weapon),
            _ when random < diamondKing => new DiamondKingEffect(weapon),
            _ when random < joker => new JokerEffect(weapon),
            _ => null
        };

        if (_cardEffect is null)
        {
            return;
        }

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