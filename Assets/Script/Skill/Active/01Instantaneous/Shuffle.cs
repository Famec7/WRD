using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shuffle : InstantaneousSkill
{
    [SerializeField]
    private ShuffleEffect _shuffleEffect;
    
    [Header("효과음")]
    [SerializeField]
    private AudioClip _diamondKingSfx;
    [SerializeField]
    private AudioClip _cloverJackSfx;
    
    private CardEffectBase _cardEffect;

    public override void OnActiveEnter()
    {
        // (클로버 잭(Data{0}), 하트 퀸(Data{1}), 다이아몬드 킹(Data{2}), 조커: Data{3}) 중 하나를 랜덤하게 선택
        float cloverJack = Data.GetValue(0);
        float heartQueen = Data.GetValue(1);
        float diamondKing = Data.GetValue(2);
        float joker = Data.GetValue(3);
        
        SpriteRenderer targetRenderer = weapon.owner.GetComponent<SpriteRenderer>();
        float random = Random.Range(0, cloverJack + heartQueen + diamondKing + joker);

        if (random < cloverJack)
        {
            _cardEffect = new CloverJackEffect(weapon);
            _shuffleEffect.PlayEffect(targetRenderer, ShuffleEffect.CardType.CloverJack);
        }
        else if (random < cloverJack + heartQueen)
        {
            _cardEffect = new HeartQueenEffect(weapon);
            _shuffleEffect.PlayEffect(targetRenderer, ShuffleEffect.CardType.HeartQueen);
        }
        else if (random < cloverJack + heartQueen + diamondKing)
        {
            _cardEffect = new DiamondKingEffect(weapon);
            _shuffleEffect.PlayEffect(targetRenderer, ShuffleEffect.CardType.DiamondKing);
        }
        else
        {
            _cardEffect = new JokerEffect(weapon);
            _shuffleEffect.PlayEffect(targetRenderer, ShuffleEffect.CardType.Joker);
        }

#if UNITY_EDITOR
        Debug.Log($"[Shuffle] {_cardEffect.GetType().Name}");
#endif
        
        Data.CoolTime = _cardEffect.Data.CoolTime;
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
        
        _cardEffect.OnExit();
        _cardEffect = null;
    }
}