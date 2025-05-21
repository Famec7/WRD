using UnityEngine;
using UnityEngine.Serialization;

public class BuddhaPress : ClickTypeSkill
{
    [SerializeField] private BuddhaHandEffect _buddhaHandEffect;
    
    [SerializeField] private AudioClip sfx;
    
    private float _damage;
    private float _stunDuration;
    float _damageAmplificationDuration;
    float _damageAmplification;

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(0);
        _stunDuration = Data.GetValue(1);

        _damageAmplificationDuration = Data.GetValue(2);
        _damageAmplification = Data.GetValue(3) / 100.0f;
        
        _buddhaHandEffect.transform.SetParent(null);
    }

    public override void OnActiveEnter()
    {
        _buddhaHandEffect.gameObject.SetActive(true);
        _buddhaHandEffect.SetPosition(ClickPosition);
        _buddhaHandEffect.PlayEffect();
        
        SoundManager.Instance.PlaySFX(sfx);
    }

    public override bool OnActiveExecute()
    {
        foreach (var monster in IndicatorMonsters)
        {
            monster.HasAttacked(_damage);
            ApplyWound(monster);
            ApplyStun(monster);
            ApplyDamageAmplification(monster);
        }
        
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
    
    private void ApplyDamageAmplification(Monster monster)
    {
        StatusEffect damageAmplification = new DamageAmplification(monster.gameObject, _damageAmplification, _damageAmplificationDuration);
        StatusEffectManager.Instance.AddStatusEffect(monster.status, damageAmplification);
    }
}