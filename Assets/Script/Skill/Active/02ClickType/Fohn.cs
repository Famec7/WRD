using System.Data.Common;
using UnityEngine;

public class Fohn : ClickTypeSkill
{
    [SerializeField] private Wind _windEffect;
    [SerializeField] private AudioClip sfx;
    
    private float _stunDuration = 0.0f;
    private float _damageAmplificationDuration = 0.0f;
    private float _damageAmplification = 0.0f;
    
    public override void OnActiveEnter()
    {
        _stunDuration = Data.GetValue(1);
        _damageAmplificationDuration = Data.GetValue(2);
        _damageAmplification = Data.GetValue(3);
        
        SoundManager.Instance.PlaySFX(sfx);
    }

    public override bool OnActiveExecute()
    {
        _windEffect.Init(Data.Range, ClickPosition, ApplyDebuff, transform);
        _windEffect.Play();
        
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
    
    private void ApplyDebuff(Monster monster)
    {
        // Stun
        StatusEffect stun = new Stun(monster.gameObject, _stunDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
        
        // Damage Amplification
        StatusEffect damageAmplificationEffect = new DamageAmplification(monster.gameObject, _damageAmplification, _damageAmplificationDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, damageAmplificationEffect);
    }
}