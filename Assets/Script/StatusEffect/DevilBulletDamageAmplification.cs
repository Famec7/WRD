using UnityEngine;

public class DevilBulletDamageAmplification : StatusEffect
{
    private readonly float _damageAmplification;
    
    public DevilBulletDamageAmplification(GameObject target, float damageAmplification, float duration = 0) : base(target, duration)
    {
        _damageAmplification = damageAmplification;
    }

    public override void ApplyEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.DevilBulletDamageAmplification += _damageAmplification;

#if STATUS_EFFECT_LOG
            Debug.Log($"DevilBulletDamageAmplification: {target.name} has {status.devilBulletDamageAmplification} devil bullets : ApplyEffect");
#endif
        }
    }

    public override void RemoveEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            status.DevilBulletDamageAmplification -= _damageAmplification;
            
#if STATUS_EFFECT_LOG
            Debug.Log($"DevilBulletDamageAmplification: {target.name} has {status.devilBulletDamageAmplification} devil bullets : RemoveEffect");
#endif
        }
    }
}