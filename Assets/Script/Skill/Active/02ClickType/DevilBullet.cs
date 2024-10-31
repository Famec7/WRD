using UnityEngine;

public class DevilBullet : ClickTypeSkill
{
    private int _attackCount = 0;
    
    protected override void OnActiveEnter()
    {
        LayerMask layerMask = LayerMaskManager.Instance.MonsterLayerMask;
        Collider2D target = Physics2D.OverlapPoint(pivotPosition, layerMask);

        if (target is null)
        {
            return;
        }

        if (target.TryGetComponent(out Monster monster))
        {
            StatusEffect markStatus = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));
            
            if (markStatus != null)
            {
                if (monster.CompareTag("Boss"))
                {
                    float attackSpeed = weapon.Data.AttackSpeed + weapon.Data.AttackSpeed * Data.GetValue(3);
                    weapon.SetAttackDelay(attackSpeed);
                    
                    weapon.AddAction(OnAttack);
                }
                else if(monster.CompareTag("Monster"))
                {
                    monster.Die();
                }
            }
            else
            {
                StatusEffect status = new Mark(monster.gameObject);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, status);
            }
            
            monster.HasAttacked(Data.GetValue(0));

            float amplification = Data.GetValue(1) / 100;
            StatusEffect devilBulletDamageAmplification = new DevilBulletDamageAmplification(monster.gameObject, amplification);
            StatusEffectManager.Instance.AddStatusEffect(monster.status, devilBulletDamageAmplification);
        }

        return;
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

    private void OnAttack()
    {
        _attackCount++;

        if (_attackCount >= Data.GetValue(2))
        {
            weapon.SetAttackDelay(weapon.Data.AttackSpeed);
            weapon.RemoveAction(OnAttack);
        }
    }
}