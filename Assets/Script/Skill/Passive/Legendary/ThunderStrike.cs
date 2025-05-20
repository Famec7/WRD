using System.Collections;
using UnityEngine;

public class ThunderStrike : PassiveSkillBase
{
    private float _damage = 0f;
    
    [SerializeField] private AudioClip sfx;

    protected override void Init()
    {
        base.Init();

        // 낙뢰 데미지
        _damage = Data.GetValue(0);
    }

    public override bool Activate(GameObject target = null)
    {
        if (CheckTrigger())
        {
            OnThunderStrike(target);

            return true;
        }

        return false;
    }

    private void OnThunderStrike(GameObject target)
    {
        Attack();

        // 전류 이펙트
        var thunderEffect = EffectManager.Instance.CreateEffect<ElectricEffect>("ThunderEffect");

        // 낙뢰 이펙트 발생
        thunderEffect.SetData(Data);
        thunderEffect.SetPosition(target.transform.position);
        thunderEffect.PlayEffect();
        
        // 사운드 재생
        SoundManager.Instance.PlaySFX(sfx);
    }

    private void Attack()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Data.Range, default,
            LayerMaskProvider.MonsterLayerMask);

        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);
            }
        }
    }
}