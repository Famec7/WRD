public class SlowZone : DebuffZone
{
    private float _slowRate;
    private float _effectDuration = 0.0f;
    
    public void SetData(float effectTime, float radius, float slowRate, float effectDuration = 0.0f)
    {
        SetData(effectTime, radius);
        _slowRate = slowRate;
        _effectDuration = effectDuration;
    }

    protected override StatusEffect ApplyStatusEffect(Status status)
    {
        SlowDown slow = new SlowDown(status.gameObject, _slowRate, _effectDuration);

        return slow;
    }
}