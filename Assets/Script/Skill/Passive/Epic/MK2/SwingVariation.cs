public class SwingVariation : HorizontalSlice
{
    private float _attackDamage;
    private float _percent;
    
    protected override void Init()
    {
        base.Init();
        
        _attackDamage = Data.GetValue(0);
        _percent = Data.GetValue(1);
    }

    protected override void TakeDamage(Monster monster)
    {
        monster.HasAttacked(_attackDamage);

        if (monster.CompareTag("Boss"))
        {
            monster.HasAttackedCurrentPercent(_percent);
        }
    }
}