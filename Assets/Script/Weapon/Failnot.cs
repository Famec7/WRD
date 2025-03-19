using System.Collections.Generic;

public class Failnot : RangedWeapon
{
    protected override void Attack()
    {
        base.Attack();

        if (owner.Target.TryGetComponent(out Monster monster))
        {
            List<Status> targets = StatusEffectManager.Instance.GetAllStatusEffects(typeof(Mark));
            targets.Remove(monster.status);
            
            foreach (var target in targets)
            {
                if (target.TryGetComponent(out Monster targetMonster))
                {
                    targetMonster.HasAttacked(Data.AttackDamage);
                }
            }
        }
    }
}