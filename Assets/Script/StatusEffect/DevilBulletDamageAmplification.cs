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
            PlayEffect(status.DevilBulletDamageAmplification);

#if STATUS_EFFECT_LOG
            Debug.Log($"DevilBulletDamageAmplification: {target.name} has {status.devilBulletDamageAmplification} devil bullets : ApplyEffect");
#endif
        }
    }

    public override void RemoveEffect()
    {
        if (target.TryGetComponent(out Status status))
        {
            float newAmplificationRate = Mathf.Round((status.DevilBulletDamageAmplification - _damageAmplification / 100.0f) * 100.0f) / 100.0f;
            status.DevilBulletDamageAmplification = newAmplificationRate;
            PlayEffect(status.DevilBulletDamageAmplification);
            
#if STATUS_EFFECT_LOG
            Debug.Log($"DevilBulletDamageAmplification: {target.name} has {status.devilBulletDamageAmplification} devil bullets : RemoveEffect");
#endif
        }
        
        StatusEffectManager.Instance.RemoveValue(status, this);
    }
    
    /// <summary>
    /// 받피증 연출 제어 함수(이펙트 제어를 Status에서 Getter Setter로 할 수 있긴 한데... 우선 시간 걸릴 거 같아서 보류)
    /// </summary>
    /// <param name="damageAmplification"></param>
    void PlayEffect(float damageAmplification)
    {
        if(damageAmplification > 0)
        {
            monsterEffecter.SetDebuffEffect(true);
        }
        else
        {
            monsterEffecter.SetDebuffEffect(false);
        }
    }
}