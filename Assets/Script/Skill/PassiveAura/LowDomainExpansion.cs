using UnityEngine;

public class LowDomainExpansion : PassiveAuraSkillBase
{
    private BoxCollider2D _collider;
    private float _slowDownValue;
    
    protected override void Init()
    {
        base.Init();
        
        // 범위 조정
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _collider.size = new Vector2(Data.Range, Data.Range);
        
        // 슬로우 비율 값
        _slowDownValue = Data.GetValue(0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tags.Contains(other.tag))
        {
            if (other.TryGetComponent(out Monster monster))
            {
                StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(monster.gameObject, _slowDownValue));
            }
        }
    }
}