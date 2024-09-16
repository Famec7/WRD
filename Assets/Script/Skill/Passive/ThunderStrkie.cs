using System.Collections;
using UnityEngine;

public class ThunderStrkie : PassiveSkillBase
{
    private WaitForSeconds _delay = null;
    private float _range = 0f;
    private float _damage = 0f;

    protected override void Init()
    {
        base.Init();

        // 전류가 잔존하는 시간
        _delay = new WaitForSeconds(Data.GetValue(1));
        // 낙뢰 범위
        _range = Data.Range;
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
        var thunderEffect = EffectManager.Instance.CreateEffect<ThunderStrikeEffect>("ThunderStrike");
        
        // 낙뢰 이펙트 발생
        thunderEffect.SetPosition(target.transform.position);
        thunderEffect.PlayEffect();
        
        Attack();

        // 전류 잔존 이펙트
        StartCoroutine(IE_ElectricEffect(target));
    }
    
    private IEnumerator IE_ElectricEffect(GameObject target)
    {
        var electricEffect = EffectManager.Instance.CreateEffect<ElectricEffect>("ElectricEffect");
        electricEffect.SetData(Data.GetValue(2), Data.GetValue(3), Data.GetValue(4));
        
        electricEffect.SetPosition(target.transform.position);
        electricEffect.SetScale(new Vector3(_range, _range, 0f));
        electricEffect.PlayEffect();

        yield return _delay;

        electricEffect.StopEffect();
    }

    private void Attack()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, Vector2.zero, _range, default,
            LayerMaskManager.Instance.MonsterLayerMask);

        foreach (var target in targets)
        {
            if(target.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);
            }
        }
    }
}