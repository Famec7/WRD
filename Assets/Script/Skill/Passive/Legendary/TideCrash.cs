using System.Collections;
using UnityEngine;

public class TideCrash : PassiveSkillBase
{
    [SerializeField] private float _attackRange = 3.0f;

    [SerializeField] private Animator _crashEffect;

    private bool _isActivated = false;

    protected override void Init()
    {
        base.Init();
        _crashEffect.transform.SetParent(null);
    }

    public override bool Activate(GameObject target = null)
    {
        if (!_isActivated && target != null)
        {
            StartCoroutine(IE_Activate(target));
            return true;
        }

        return false;
    }

    private IEnumerator IE_Activate(GameObject target)
    {
        _isActivated = true;

        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, _attackRange, default, targetLayer);
            
            _crashEffect.gameObject.SetActive(true);
            _crashEffect.transform.position = targetPosition;
            _crashEffect.Play("TideCrash");

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Monster monster))
                {
                    monster.HasAttacked(Data.GetValue(1));
                    
                    StatusEffect preventWoundConsumption = new PreventWoundConsumption(monster.gameObject, Data.GetValue(2));
                    StatusEffectManager.Instance.AddStatusEffect(monster.status, preventWoundConsumption);
                }
            }
        }

        yield return new WaitForSeconds(Data.GetValue(0));
        
        _crashEffect.gameObject.SetActive(false);
        _isActivated = false;
    }
}