public class HighVisionStrikeVariation : HighVisionStrike
{
    protected override void Attack(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(0));
        
        Stun stun = new Stun(monster.gameObject, Data.GetValue(1));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
        
        DamageAmplification amplitude = new DamageAmplification(monster.gameObject, Data.GetValue(2));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, amplitude);
    }
}