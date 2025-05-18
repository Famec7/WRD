using System;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleEffect : MonoBehaviour
{
    public enum CardType
    {
        CloverJack,
        HeartQueen,
        DiamondKing,
        Joker
    }
    
    private Animator _animator;
    
    [SerializeField]
    private List<AnimationClip> _animationClips;

    private void Awake()
    {
        if (TryGetComponent(out _animator) == false)
        {
            Debug.LogError($"[ShuffleEffect] {gameObject.name}에 Animator가 없습니다.");
            return;
        }
    }

    public void PlayEffect(SpriteRenderer targetRenderer, CardType cardType)
    {
        if (targetRenderer == null)
        {
            Debug.LogError($"[ShuffleEffect] {gameObject.name}의 Target이 없습니다.");
            return;
        }
        
        transform.position = targetRenderer.bounds.center + new Vector3(0, targetRenderer.bounds.extents.y, 0);
        transform.SetParent(targetRenderer.transform);
        gameObject.SetActive(true);
        
        _animator.Play(_animationClips[(int)cardType].name);
    }
    
    public void StopEffect()
    {
        gameObject.SetActive(false);
    }
}