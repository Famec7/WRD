using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeuxferVariation : Deuxfer
{
    public bool isMaxHpBasedDamage = false;
    protected override void TakeDamage(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(0));
        if(isMaxHpBasedDamage)
            monster.HasAttackedCurrentPercent(Data.GetValue(1));
        else
            monster.HasAttackedPercent(Data.GetValue(1));

        float stunDuration = Data.GetValue(2);
        Status status = monster.status;
        StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, 100f, stunDuration));

    }
}
