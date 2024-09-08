using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpBlade : InstantaneousSkill
{
    #region Data
    private float _damage;
    private float _woundDamge;
    private Vector3 _range = new Vector3(0.75f, 1.75f,1);
    #endregion

    /*******Effect*******/
    [SerializeField] private EffectBase _effect;

    protected override void Init()
    {
        base.Init();
        SetData();   
    }

    private void SetData()
    {
        _damage = Data.GetValue(0);
        _woundDamge = Data.GetValue(1);
    }

    // Start is called before the first frame update
    protected override void OnActiveEnter()
    {
    }

    protected override INode.ENodeState OnActiveExecute()
    {

        if (weapon.owner.Target == null)
        {
            IsActive = false;
            return INode.ENodeState.Failure;
        }

        Vector3 dir = weapon.owner.Target.transform.position - weapon.owner.transform.position;
        if (dir == Vector3.zero)
        {

            IsActive = false;
            return INode.ENodeState.Failure;
        }

        List<Collider2D> targets = RangeDetectionUtility.GetAttackTargets(weapon.owner.transform.position, _range, dir, targetLayer);

        if (targets.Count == 0)
        {
            IsActive = false;
            return INode.ENodeState.Failure;
        }

        // 이펙트 재생
        _effect.SetPosition(weapon.owner.transform.position + dir);
        _effect.SetRotation(Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.left, dir)));
        _effect.SetScale(_range);
        _effect.PlayEffect();

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                Debug.Log("요기요3");

                var wound = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Wound));
                monster.HasAttacked(_damage);

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
