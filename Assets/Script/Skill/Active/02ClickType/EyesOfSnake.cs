public class EyesOfSnake : ClickTypeSkill
{
    private float _duration = 0.0f;
    private float _damageAmplification = 0.0f;
    private float _slowRate = 0.0f;
    
    public override void OnActiveEnter()
    {
        _duration = Data.GetValue(0);
        _damageAmplification = Data.GetValue(1) / 100.0f;
        _slowRate = Data.GetValue(2);
    }

    public override bool OnActiveExecute()
    {
        Monster target = SelectMonsterAtClickPosition();
        
        if (target is null)
        {
            return true;
        }
        
        ApplyEffect(target);
        
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
    
    private void ApplyEffect(Monster target)
    {
        StatusEffect markEffect = new Mark(target.gameObject, _duration);
        StatusEffectManager.Instance.AddStatusEffect(target.status, markEffect);
            
        StatusEffect damageAmplificationEffect = new DamageAmplification(target.gameObject, _damageAmplification, _duration);
        StatusEffectManager.Instance.AddStatusEffect(target.status, damageAmplificationEffect);
            
        StatusEffect slowEffect = new SlowDown(target.gameObject, _slowRate, _duration);
        StatusEffectManager.Instance.AddStatusEffect(target.status, slowEffect);
        
        MonsterEffecter monsterEffecter = target.GetComponentInChildren<MonsterEffecter>();
        if (monsterEffecter != null)
        {
            monsterEffecter.SetSnakeEffect(true, _duration);
        }
    }
}