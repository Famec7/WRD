using UnityEngine;

public class MoveState : IState<PlayerController>
{
    public void Enter(PlayerController entity)
    {
        ;
    }

    public void Execute(PlayerController entity)
    {
        bool isPointerOverUI = UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("UI"));
        bool isPointerOverSkillUI = UIHelper.IsPointerOverUILayer(LayerMask.NameToLayer("SkillUI"));
        if (isPointerOverUI || isPointerOverSkillUI)
        { 
            return;
        }
        
        LayerMask layerMask = LayerMaskProvider.MonsterLayerMask;
        Collider2D col = Physics2D.OverlapPoint(entity.TouchPos, layerMask);
        if (col is null)
        {
            return;
        }
        
        Vector3 dir = (entity.TouchPos - entity.transform.position).normalized;
        entity.transform.position += dir * (entity.Data.MoveSpeed * Time.deltaTime);
    }

    public void Exit(PlayerController entity)
    {
        ;
    }
}