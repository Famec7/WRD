public class SwingVariation : HorizontalSlice
{
    private float _skillDamage;
    private float _percent;
    
    protected override void Init()
    {
        base.Init();
        
        _skillDamage = Data.GetValue(0);
        _percent = Data.GetValue(1);
    }

    protected override void TakeDamage(Monster monster)
    {
        monster.HasAttacked(_skillDamage);

        if (monster.CompareTag("Boss"))
        {
            monster.HasAttackedPercent(_percent);
        }
    }
}