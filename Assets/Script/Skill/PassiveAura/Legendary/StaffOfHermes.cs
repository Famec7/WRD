using System;

public class StaffOfHermes : PassiveAuraSkillBase
{
    CharacterController player;
    
    protected override void Init()
    {
        base.Init();
        
        player = CharacterManager.Instance.GetCharacter(CharacterManager.CharacterType.Player);
    }
    
    private void OnEnable()
    {
        player.Data.MoveSpeed += player.Data.MoveSpeed * (Data.GetValue(0) / 100.0f);
    }
    
    private void OnDisable()
    {
        player.Data.MoveSpeed -= player.Data.MoveSpeed * (Data.GetValue(0) / 100.0f);
    }
}