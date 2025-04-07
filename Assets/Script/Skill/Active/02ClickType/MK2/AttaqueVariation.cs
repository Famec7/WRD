using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttaqueVariation : Attaque
{
    protected override void TakeDamage(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(0));
        monster.HasAttackedPercent(Data.GetValue(1));
        StartCoroutine(ChangePassiveEffect());
        StartCoroutine(BoostAttackSpeed());
    }

    IEnumerator ChangePassiveEffect()
    {
        weapon.GetPassiveSkill().GetComponent<DeuxferVariation>().isMaxHpBasedDamage = true;
        yield return new WaitForSeconds(Data.GetValue(2));
        weapon.GetPassiveSkill().GetComponent<DeuxferVariation>().isMaxHpBasedDamage = false;
    }

    IEnumerator BoostAttackSpeed()
    {
        float originAttackSpeed = weapon.Data.AttackSpeed;
        weapon.SetAttackDelay(originAttackSpeed * (1 + Data.GetValue(3)));
        yield return new WaitForSeconds(Data.GetValue(2));
        weapon.SetAttackDelay(originAttackSpeed);
    }
}
