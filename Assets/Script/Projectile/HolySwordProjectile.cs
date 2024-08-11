using UnityEngine;

public class HolySwordProjectile : FallingSwordProjectile
{
    #region Data

    private float _stunTime = 0.0f; // 스턴 시간
    private float _damageIncreaseRate = 0.0f; // 데미지증가율
    
    public override void SetData(PassiveSkillData data)
    {
        base.SetData(data);
        
        Damage = data.GetValue(0);
        _stunTime = data.GetValue(1);
        
        if(Dealy is null)
            Dealy = new WaitForSeconds(data.GetValue(2));
    }

    #endregion
    

    protected override void OnSwordImpact()
    {
        throw new System.NotImplementedException();
    }
}