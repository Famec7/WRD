public class ElectricZone : DebuffZone
{
    private float _damageAmplification = 0f;
    private float _shockDuration = 0f;
    
    public void SetData(float effectTime, float radius, float damageAmplification, float shockDuration)
    {
        SetData(effectTime, radius);
        _damageAmplification = damageAmplification;
        _shockDuration = shockDuration;
    }
    
    protected override StatusEffect ApplyStatusEffect(Status status)
    {
        if (status.WoundStack > 0)
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(Wound));
            ElectricShock electricShock = new ElectricShock(status.gameObject, _damageAmplification, _shockDuration);
            
            return electricShock;
        }

        return null;
    }
}