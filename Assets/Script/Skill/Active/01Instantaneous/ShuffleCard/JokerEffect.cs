using System.Collections.Generic;
using UnityEngine;

public class JokerEffect : CardEffectBase
{
    private readonly List<CardEffectBase> _cardEffects;
    private List<bool> _isComplete;
    
    public JokerEffect(WeaponBase weapon, AudioClip sfx = null) : base(weapon, sfx)
    {
        Data = SkillManager.Instance.GetActiveSkillData(14);
        
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

    public override bool OnUpdate()
    {
        bool isAllComplete = true;
        
        for (int i = 0; i < _cardEffects.Count; i++)
        {
            if (!_isComplete[i])
            {
                bool result = _cardEffects[i].OnUpdate();
                if (result)
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
        
        return isAllComplete ? true : false;
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