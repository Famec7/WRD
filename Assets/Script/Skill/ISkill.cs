public interface ISkill
{
    void CastingEnter();
    void CastingUpdate();
    void CastingExit();
    void CooldownEnter();
    void CooldownUpdate();
    void CooldownExit();
    void ReadyEnter();
    void ReadyUpdate();
    void ReadyExit();
}