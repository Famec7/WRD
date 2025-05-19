using UnityEngine;

public class HighVisionStrike : PassiveSkillBase
{
    [SerializeField]
    private AudioClip _strikeSound;
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
            return false;
        
        Vector3 targetPosition = target.transform.position;
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, LayerMaskProvider.MonsterLayerMask);
        
        if (targets.Count == 0)
            return false;
        
        SoundManager.Instance.PlaySFX(_strikeSound);
        
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("HighVisionStrike");
        effect.SetPosition(targetPosition);
        effect.PlayEffect();
        
        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                Attack(monster);
            }
        }
        
        return true;
    }

    protected virtual void Attack(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(0));
        StatusEffectManager.Instance.AddStatusEffect(monster.status, new Stun(monster.gameObject, Data.GetValue(1)));
    }
}