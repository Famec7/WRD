public class ActiveSkillCommand : ICommand
{
    private readonly ActiveSkillBase _skill;
    private bool _isExecute = false;
    
    public ActiveSkillCommand(ActiveSkillBase skill)
    {
        _skill = skill;
        
        SkillUIManager.Instance.ShowPopupPanel(3);
        _skill.OnButtonActivate?.Invoke(false);
        
        _skill.OnActiveEnter();
    }
    
    public bool Execute()
    {
        return _skill.OnActiveExecute();
    }
    
    public void OnComplete()
    {
        _skill.OnActiveExit();
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
        
        _skill.AddCommand(new CooldownCommand(_skill));
    }

    public void Undo()
    {
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
        _skill.OnActiveExit();
        
        _skill.AddCommand(new CooldownCommand(_skill));
    }
}