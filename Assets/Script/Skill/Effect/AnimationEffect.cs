using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEffect : EffectBase
{
    private Animator _animator;
    
    [SerializeField]
    private AnimationClip _clip;
    
    protected override void Init()
    {
        if (TryGetComponent(out _animator) == false)
        {
            Debug.LogError("Animator 컴포넌트가 없습니다.");
        }
    }

    public override void PlayEffect()
    {
        _animator.Play(_clip.name);
    }

    public override void StopEffect()
    {
        if (_animator != null)
        {
            _animator.enabled = false;
        }
        
        EffectManager.Instance.ReturnEffectToPool(this, _clip.name);
    }
    
    public override void GetFromPool()
    {
        base.GetFromPool();
        _animator.enabled = true;
    }
}