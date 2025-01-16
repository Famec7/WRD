public class EyesOfSnake : ClickTypeSkill
{
    public override void OnActiveEnter()
    {
        FindTarget();

        if (target.TryGetComponent(out Monster monster))
        {
            float duration = Data.GetValue(0);
            float damageAmplification = Data.GetValue(1) / 100.0f;
            float slowRate = Data.GetValue(2);
            
            StatusEffect damageAmplificationEffect = new DamageAmplification(monster.gameObject, damageAmplification, duration);
            StatusEffectManager.Instance.AddStatusEffect(monster.status, damageAmplificationEffect);
            
            StatusEffect slowEffect = new SlowDown(monster.gameObject, slowRate, duration);
            StatusEffectManager.Instance.AddStatusEffect(monster.status, slowEffect);
        }
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }
}