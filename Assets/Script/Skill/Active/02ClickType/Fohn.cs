using System.Data.Common;
using UnityEngine;

public class Fohn : ClickTypeSkill
{
    [SerializeField] private Wind _windEffect;
    
    public override void OnActiveEnter()
    {
        float radius = Data.Range;
        
        _windEffect.Init(radius, ClickPosition, ApplyDebuff);
        _windEffect.Play();
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
    
    private void ApplyDebuff(Monster monster)
    {
        // Stun
        float stunDuration = Data.GetValue(1);
        StatusEffect stun = new Stun(monster.gameObject, stunDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
        
        // Damage Amplification
        float damageAmplificationDuration = Data.GetValue(2);
        float damageAmplification = Data.GetValue(3);
        StatusEffect damageAmplificationEffect = new DamageAmplification(monster.gameObject, damageAmplification, damageAmplificationDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, damageAmplificationEffect);
    }
}