using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : ClickTypeSkill
{
    [SerializeField]
    private AudioClip _fireSound;
    
    public override void OnActiveEnter()
    {
        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        Collider2D target = Physics2D.OverlapPoint(PivotPosition, layerMask);

        if (target != null && target.TryGetComponent(out Monster monster))
        {
            StatusEffect markStatus = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

            if (markStatus is null)
            {
                OnAttackMultipleTargets(targetMonsters.ToList());
                return;
            }

            OnAttackSingleTarget(monster);
        }
        else
        {
            OnAttackMultipleTargets(targetMonsters.ToList());
        }
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        
    }

    private void OnAttackSingleTarget(Monster monster)
    {
        monster.HasAttacked(Data.GetValue(0));
        
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
        effect.SetPosition(monster.transform.position);
        effect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(_fireSound);
    }

    private void OnAttackMultipleTargets(List<Monster> monsters)
    {
        foreach (var monster in monsters)
        {
            monster.HasAttacked(Data.GetValue(3));
            
            Status status = monster.status;
            float slowDuration = Data.GetValue(1);
            float slowRate = Data.GetValue(2);
            
            StatusEffectManager.Instance.AddStatusEffect(status, new SlowDown(status.gameObject, slowRate, slowDuration));
            
            ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
            effect.SetPosition(monster.transform.position);
            effect.PlayEffect();
            
            SoundManager.Instance.PlaySFX(_fireSound);
        }
    }
}