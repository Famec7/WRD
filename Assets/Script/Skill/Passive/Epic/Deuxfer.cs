using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Deuxfer : PassiveSkillBase
{
    [SerializeField]
    private Material _originalMaterial;

    [SerializeField]
    private Material _blueMaterial;

    [SerializeField]
    private float effectDuration = 0.5f;

    [SerializeField] private SpriteRenderer _spriteRenderer;

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
        _spriteRenderer.material = _blueMaterial;
        yield return new WaitForSeconds(effectDuration);
        _spriteRenderer.material = _originalMaterial;
    }


}
