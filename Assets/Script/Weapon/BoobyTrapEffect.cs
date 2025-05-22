using UnityEngine;

public class BoobyTrapEffect : ParticleEffect
{
    [SerializeField] private SlowZone slowZone;
    [SerializeField] private DamageAmplificationZone damageAmplificationZone;
    
    public void SetData(float effectTime, float radius, float damageAmplificationRate, float slowRate)
    {
        slowZone.SetData(effectTime, radius, slowRate);
        damageAmplificationZone.SetData(effectTime, radius, damageAmplificationRate);
    }
    
    public void PlayDebuff()
    {
        slowZone.PlayEffect();
        damageAmplificationZone.PlayEffect();
    }
}