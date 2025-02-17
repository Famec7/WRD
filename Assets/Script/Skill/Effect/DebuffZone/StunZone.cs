public class StunZone : DebuffZone
{
    private float _effectDuration;
    
    public void SetData(float effectTime, float radius, float effectDuration = 0.0f)
    {
        base.SetData(effectTime, radius);
        _effectDuration = effectDuration;
    }
    
    protected override StatusEffect ApplyStatusEffect(Status status)
    {
        Stun stun = new Stun(status.gameObject, _effectDuration);
        return stun;
    }
}