using System;
using UnityEngine;
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

    public override void OnActiveEnter()
    {
        // (클로버 잭(Data{0}), 하트 퀸(Data{1}), 다이아몬드 킹(Data{2}), 조커: Data{3}) 중 하나를 랜덤하게 선택
        float cloverJack = Data.GetValue(0);
        float heartQueen = Data.GetValue(1);
        float diamondKing = Data.GetValue(2);
        float joker = Data.GetValue(3);

        float random = Random.Range(0, cloverJack + heartQueen + diamondKing + joker);

        if (random < cloverJack)
        {
            _cardEffect = new CloverJackEffect(weapon);
        }
        else if (random < cloverJack + heartQueen)
        {
            _cardEffect = new HeartQueenEffect(weapon);
        }
        else if (random < cloverJack + heartQueen + diamondKing)
        {
            _cardEffect = new DiamondKingEffect(weapon);
        }
        else
        {
            _cardEffect = new JokerEffect(weapon);
        }

#if UNITY_EDITOR
        Debug.Log($"[Shuffle] {_cardEffect.GetType().Name}");
#endif
        
        _cardEffect.OnEnter();
    }

    public override bool OnActiveExecute()
    {
        bool result = _cardEffect.OnUpdate();

        return result;
    }

    public override void OnActiveExit()
    {
        if (_cardEffect == null)
        {
            return;
        }

        Data.CoolTime = _cardEffect.Data.CoolTime;
        _cardEffect.OnExit();
        _cardEffect = null;
    }
}