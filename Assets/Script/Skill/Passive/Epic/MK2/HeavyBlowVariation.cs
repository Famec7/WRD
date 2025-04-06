public class HeavyBlowVariation : HeavyBlow
{
    protected override void TakeWoundDamage(Monster monster)
    {
        monster.HasAttackedPercent(Data.GetValue(2));
    }
}