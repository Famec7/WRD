using UnityEngine;

public class SilentTrueshot : ClickTypeSkill
{
    private float _damageDelay = 0.0f;
    private float _damage = 0.0f;

    [SerializeField] private Animator _effect;
    [SerializeField] private AudioClip sfx;

    protected override void Init()
    {
        base.Init();
        
        _damage = Data.GetValue(1);
    }

    public override void OnActiveEnter()
    {
        _damageDelay = Data.GetValue(0);
        ClearTargetMonsters();
        
        weapon.enabled = false;
        weapon.owner.enabled = false;
        
        if (_effect != null)
        {
            _effect.gameObject.SetActive(true);
            _effect.transform.position = ClickPosition;
            _effect.Play("ActiveEffect");
        }
        
        SoundManager.Instance.PlaySFX(sfx);
    }

    public override bool OnActiveExecute()
    {
        if (_damageDelay > 0.0f)
        {
            _damageDelay -= Time.deltaTime;
            return false;
        }
        
        foreach (var monster in IndicatorMonsters)
        {
            if (HasTargetMark(monster))
            {
                monster.HasAttacked(_damage);
            }
        }

        return true;
    }

    public override void OnActiveExit()
    {
        weapon.enabled = true;
        weapon.owner.enabled = true;
        _effect.gameObject.SetActive(false);
    }

    private bool HasTargetMark(Monster monster)
    {
        StatusEffect mark = StatusEffectManager.Instance.GetStatusEffect(monster.status, typeof(Mark));

        return mark != null;
    }
}