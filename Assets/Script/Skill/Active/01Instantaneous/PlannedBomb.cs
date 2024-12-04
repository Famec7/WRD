using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlannedBomb : InstantaneousSkill
{
    [SerializeField]
    private AudioClip _bombSound;
    
    protected override void OnActiveEnter()
    {
        GlueBomb passiveAuraSkill = (GlueBomb)weapon.passiveAuraSkill;
        var bombList = passiveAuraSkill.BombProjectiles.ToArray();

        foreach (var bomb in bombList)
        {
            Explosion(bomb.transform.position);
        }

        // 딕셔너리 초기화
        passiveAuraSkill.monsterBombs.Clear();
        
        SoundManager.Instance.PlaySFX(_bombSound);
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        ;
    }

    private void Explosion(Vector3 targetPos)
    {
        var targets = RangeDetectionUtility.GetAttackTargets(targetPos, Data.Range, default, targetLayer);

        if (targets.Count == 0)
            return;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));

                StatusEffect stun = new SlowDown(monster.gameObject, 100f, Data.GetValue(1));
                StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);

                ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
                effect.SetPosition(monster.transform.position);
            }
        }
    }
}

