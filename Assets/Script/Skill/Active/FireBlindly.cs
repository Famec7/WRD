public class FireBlindly : Skill
{
    private int count;
    #region Ready

    public override void ReadyEnter()
    {
        base.ReadyEnter();
    }

    public override void ReadyUpdate()
    {
        base.ReadyUpdate();
    }

    public override void ReadyExit()
    {
        base.ReadyExit();
    }

    #endregion

    #region Casting
    public override void CastingEnter()
    {
        base.CastingEnter();
    }

    public override void CastingUpdate()
    {
        base.CastingUpdate();
    }

    public override void CastingExit()
    {
        base.CastingExit();
    }
    #endregion

    #region Active
    
    public override void ActiveEnter()
    {
        base.ActiveEnter();
        //To do: 무기 스크립트의 기본 공격 데이터를 조정(공속 3)
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
    }

    public override void ActiveExit()
    {
        base.ActiveExit();
        //To do: 무기 스크립트의 기본 공격 데이터를 원래대로 조정(공속 1)
        //To do: 2초 동안 공격 불가능하도록 조정
    }

    #endregion

    #region Cooldown
    
    public override void CooldownEnter()
    {
        base.CooldownEnter();
    }

    public override void CooldownUpdate()
    {
        base.CooldownUpdate();
    }

    public override void CooldownExit()
    {
        base.CooldownExit();
    }

    #endregion
}