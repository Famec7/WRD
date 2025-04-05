public class SharpBladeVariation : SharpBlade
{
    protected override void TakeWoundDamage(Monster monster)
    {
        monster.HasAttackedPercent(WoundDamage);
    }
}