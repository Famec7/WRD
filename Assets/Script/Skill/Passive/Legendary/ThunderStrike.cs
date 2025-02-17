using System.Collections;
using UnityEngine;

public class ThunderStrike : PassiveSkillBase
{
    private WaitForSeconds _delay = null;
    private float _range = 0f;
    private float _damage = 0f;
    
    [SerializeField]
    private SlowZone _slowZone;
    [SerializeField]
    private ElectricZone _electricZone;

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
        thunderEffect.SetData(Data);
        thunderEffect.SetPosition(target.transform.position);
        thunderEffect.PlayEffect();

        yield return _delay;

        thunderEffect.StopEffect();
    }

    private void Attack()
    {
        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, _range, default,
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