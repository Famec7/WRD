using System.Collections.Generic;
using UnityEngine;

public abstract class ClickTypeSkill : ActiveSkillBase
{
    public override void UseSkill()
    {
        IsCoolTime = false;
        IsUsuableRangeState = true;
    }

    /***************************Behaviour Tree***************************/

    #region Behaviour Tree

    protected override INode SettingBT()
    {
        return new SelectorNode(
            new List<INode>()
            {
                new SequenceNode(
                    CoolTimeNodes()
                ),
                new SequenceNode(
                    UsableRangeNodes()
                ),
                new SequenceNode(
                    IndicatorNodes()
                ),
                new SequenceNode(
                    ActiveNodes()
                ),
            }
        );
    }

    #region Usable Range Node

    protected virtual List<INode> UsableRangeNodes()
    {
        return new List<INode>
        {
            new ActionNode(CheckUsuableRangeState),
            new ActionNode(TouchUsableRange),
            new ActionNode(CheckingUsableRange),
        };
    }

    private bool _isUsuableRangeState = false;

    protected bool IsUsuableRangeState
    {
        set
        {
            _isUsuableRangeState = value;

            if (_isUsuableRangeState is false)
            {
                IndicatorManager.Instance.HideUsableIndicator();
            }
            else
            {
                IndicatorManager.Instance.ShowUsableIndicator(weapon.owner.transform.position, Data.AvailableRange);

                weapon.owner.enabled = false;

                preparingTime = 3f;
                pivotPosition = default;

                SkillUIManager.Instance.ShowPopupPanel();
            }
        }

        get => _isUsuableRangeState;
    }

    private INode.ENodeState CheckUsuableRangeState()
    {
        return IsUsuableRangeState is true ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    private INode.ENodeState TouchUsableRange()
    {
        preparingTime -= Time.deltaTime;
        if (preparingTime <= 0f)
        {
            CancelSkill();
            return INode.ENodeState.Failure;
        }

        if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("SkillUI")))
        {
            return INode.ENodeState.Running;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // UI 터치 시 스킬 취소
            if (UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("UI")))
            {
                CancelSkill();
                return INode.ENodeState.Failure;
            }

            pivotPosition = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            return INode.ENodeState.Success;
        }

        return INode.ENodeState.Running;
    }

    private INode.ENodeState CheckingUsableRange()
    {
        IsUsuableRangeState = false;
        float distanceFromPlayerToPivot = Vector2.Distance(weapon.owner.transform.position, pivotPosition);

        if (distanceFromPlayerToPivot > Data.AvailableRange / 2)
        {
            CancelSkill();
            return INode.ENodeState.Failure;
        }

        var currentSettingType = SettingManager.Instance.CurrentActiveSettingType;
        switch (currentSettingType)
        {
            case SettingManager.ActiveSettingType.Auto:
                IsCoolTime = true;
                break;
            case SettingManager.ActiveSettingType.SemiAuto:
                IsActive = true;
                break;
            case SettingManager.ActiveSettingType.Manual:
                IsIndicatorState = true;
                break;
            default:
                break;
        }
        
        Vector2 direction =  (Vector2)weapon.owner.transform.position - pivotPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        SkillIndicator indicator = IndicatorManager.Instance.GetIndicator(indicatorType);
        indicator.transform.rotation = Quaternion.Euler(0, 0, angle);

        return INode.ENodeState.Success;
    }

    #endregion

    #endregion

    public override void CancelSkill()
    {
        base.CancelSkill();
        IsUsuableRangeState = false;
    }

    protected GameObject target;
    
    protected void FindTarget()
    {
        if (SettingManager.Instance.CurrentActiveSettingType == SettingManager.ActiveSettingType.Auto)
        {
            if (weapon.owner.Target is null)
            {
                target = weapon.owner.FindNearestTarget();
            }
            else if (weapon.owner.Target.TryGetComponent(out Monster monster))
            {
                target = monster.gameObject;
            }
            else
            {
                target = weapon.owner.FindNearestTarget();
            }
        }
        else
        {
            LayerMask layerMask = LayerMaskManager.Instance.MonsterLayerMask;
            Collider2D collider = Physics2D.OverlapPoint(pivotPosition, layerMask);
            target = collider != null ? collider.gameObject : null;
        }
    }
}