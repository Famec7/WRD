using UnityEngine;

public class BuddhaPress : ClickTypeSkill
{
    [SerializeField] private BrokenGround _brokenGround;
    
    private float _damage;
    private float _stunDuration;

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(0);
        _stunDuration = Data.GetValue(1);
        
        float brokenGroundTime = Data.GetValue(2);
        float damageAmplification = Data.GetValue(3);
        _brokenGround.SetData(brokenGroundTime, damageAmplification, Data.Range);
    }

    public override void OnActiveEnter()
    {
        _brokenGround.transform.SetParent(null);
    }

    public override bool OnActiveExecute()
    {
        foreach (var monster in IndicatorMonsters)
        {
            ApplyWound(monster);
            monster.HasAttacked(_damage);
            ApplyStun(monster);
            
        }
        
        _brokenGround.SetPosition(ClickPosition);
        _brokenGround.PlayEffect();
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    private void ApplyWound(Monster monster)
    {
        StatusEffect wound = new Wound(monster.gameObject);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, wound);
    }
    
    private void ApplyStun(Monster monster)
    {
        StatusEffect stun = new Stun(monster.gameObject, _stunDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, stun);
    }
}