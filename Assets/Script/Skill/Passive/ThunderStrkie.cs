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
        Attack();

        // 전류 이펙트
        StartCoroutine(IE_ElectricEffect(target));
    }
    
    private IEnumerator IE_ElectricEffect(GameObject target)
    {
        var thunderEffect = EffectManager.Instance.CreateEffect<ElectricEffect>("ThunderEffect");
        
        // 낙뢰 이펙트 발생
        thunderEffect.SetPosition(target.transform.position);
        thunderEffect.SetData(Data.GetValue(2), Data.GetValue(3), Data.GetValue(4));
        thunderEffect.SetScale(new Vector3(_range, _range, _range));
        thunderEffect.PlayEffect();
        
        thunderEffect.PlayEffect();

        yield return _delay;

        thunderEffect.StopEffect();
    }

    private void Attack()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, _range, default,
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