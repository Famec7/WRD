using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deuxfer : PassiveSkillBase
{
    [SerializeField]
    private Material _originalMaterial;

    [SerializeField]
    private Material _blueMaterial;

    [SerializeField]
    private float effectDuration = 0.5f;
    

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        StartCoroutine(ApplyBlueEffect());

        if (weapon.owner.Target.TryGetComponent(out Monster monster))
        {
            monster.HasAttacked(weapon.Data.AttackDamage);
            ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            effect.SetPosition(weapon.owner.Target.transform.position);
        }

        return true;
    }

    private IEnumerator ApplyBlueEffect()
    {
        weapon.GetComponent<SpriteRenderer>().material = _blueMaterial;
        yield return new WaitForSeconds(effectDuration);
        weapon.GetComponent<SpriteRenderer>().material = _originalMaterial;
    }


}
