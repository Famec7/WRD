public class DamageAmplificationZone : DebuffZone
{
    private float _damageAmplification;
    private float _effectDuration = 0.0f;
    
    public void SetData(float effectTime, float radius, float damageAmplification, float effectDuration = 0.0f)
    {
        SetData(effectTime, radius);
        _damageAmplification = damageAmplification;
        _effectDuration = effectDuration;
    }
    
    protected override StatusEffect ApplyStatusEffect(Status status)
    {
        StatusEffect damageAmplification = new DamageAmplification(status.gameObject, _damageAmplification, _effectDuration);

        return damageAmplification;
    }
}