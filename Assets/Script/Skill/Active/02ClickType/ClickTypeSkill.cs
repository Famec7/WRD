using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            new ActionNode(TouchUsableRange),
            new ActionNode(CheckingUsableRange),
        };
    }

    private bool _isUsuableRangeState;

    protected bool IsUsuableRangeState
    {
        set
        {
            _isUsuableRangeState = value;

            if (_isUsuableRangeState is false)
            {
                HideUsableRange();
            }
            else
            {
                ShowUsableRange();

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

        if (Input.GetMouseButtonUp(0))
        {
            // UI 터치 시 스킬 취소
            if (EventSystem.current.IsPointerOverGameObject())
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
        float distanceFromPlayerToPivot = Vector2.Distance(owner.transform.position, pivotPosition);

        if (distanceFromPlayerToPivot > Data.AvailableRange)
        {
            CancelSkill();
            return INode.ENodeState.Failure;
        }

        indicator.transform.position = pivotPosition;
        
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

        return INode.ENodeState.Success;
    }

    #endregion

    #endregion
}