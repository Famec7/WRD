using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Deuxfer : PassiveSkillBase
{
    
    public Material _originalMaterial;

    
    public Material _blueMaterial;

    public float effectDuration = 0.5f;

    public SpriteRenderer _spriteRenderer;

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null) return false;
        StartCoroutine(ApplyBlueEffect());

        if (target.TryGetComponent(out Monster monster))
        {
            TakeDamage(monster);
            ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("NormalHit");
            effect.SetPosition(target.transform.position);
        }

        return true;
    }

    private IEnumerator ApplyBlueEffect()
    {
        _spriteRenderer.material = _blueMaterial;
        yield return new WaitForSeconds(effectDuration);
        _spriteRenderer.material = _originalMaterial;
    }

    protected virtual void TakeDamage(Monster monster)
    {
        monster.HasAttacked(weapon.Data.AttackDamage);
    }

}
