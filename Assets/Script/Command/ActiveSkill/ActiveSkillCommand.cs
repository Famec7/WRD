public class ActiveSkillCommand : ICommand
{
    private readonly ActiveSkillBase _skill;
    private bool _isExecute = false;
    
    public ActiveSkillCommand(ActiveSkillBase skill)
    {
        _skill = skill;
        
        SkillUIManager.Instance.ShowPopupPanel(3);
        _skill.OnButtonActivate?.Invoke(false);
        _skill.IsActive = true;
        
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
        _skill.IsActive = false;
        
        _skill.ExecuteCoolTimeCommand();
    }

    public void Undo()
    {
        _skill.OnActiveExit();
        _skill.CurrentCoolTime = _skill.Data.CoolTime;
        _skill.IsActive = false;
        
        _skill.ExecuteCoolTimeCommand();
    }
}