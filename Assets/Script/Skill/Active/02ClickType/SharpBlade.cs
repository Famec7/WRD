using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBlade : ClickTypeSkill
{
    #region Data
    private float _damage;
    private float _woundDamge;
    private Vector2 _range = new Vector2(0.75f, 1.75f);
    #endregion

    /*******Effect*******/
    [SerializeField] private EffectBase _effect;

    private void SetData()
    {
        _damage = Data.GetValue(0);
        _woundDamge = Data.GetValue(1);
    }

    // Start is called before the first frame update
    protected override void OnActiveEnter()
    {
            Debug.Log("요기요5");
    }

    protected override INode.ENodeState OnActiveExecute()
    {
        Debug.Log("요기요");

        if (weapon.owner.Target == null)
        {
            Debug.Log("요기요");
            IsActive = false;
            return INode.ENodeState.Failure;
        }

        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;
        if (dir == Vector3.zero)
        {
            Debug.Log("요기요2");

            IsActive = false;
            return INode.ENodeState.Failure;
        }

        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _range, dir, targetLayer);

        if (targets.Count == 0)
        {
            Debug.Log("요기요3");

            IsActive = false;
        }
        Debug.Log("요기요4");

        // 이펙트 재생
        _effect.SetPosition(weapon.owner.transform.position + dir);
        _effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.left, dir)));
        _effect.SetScale(_range);
        _effect.PlayEffect();

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                var wound = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));

                if (wound is null) continue;

                StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(Wound));
                monster.HasAttacked(_woundDamge);
            }
        }

        IsActive = false;
        return INode.ENodeState.Success;
    }

    protected override void OnActiveExit()
    {
        ;
    }

  

}
