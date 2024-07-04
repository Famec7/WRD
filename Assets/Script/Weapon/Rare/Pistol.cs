public class Pistol : RangedWeapon
{
    protected override void Attack()
    {
        base.Attack();
        
        if (owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(Data.attackDamage);
        }
    }
}