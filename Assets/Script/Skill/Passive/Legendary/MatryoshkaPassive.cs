using UnityEngine;

public class MatryoshkaPassive : PassiveSkillBase
{
    [SerializeField] private float stunRange = 3.0f;

    private float _slowRange = 0.0f;
    public float SlowRange
    {
        set
        {
            _slowRange = value;
            _collider.radius = _slowRange / 2;            
        }
    }
    
    private CircleCollider2D _collider;
    
    protected override void Init()
    {
        base.Init();
        
        targetLayer = LayerMaskManager.Instance.MonsterLayerMask;
        _collider = GetComponent<CircleCollider2D>();

        SlowRange = 0.0f;
    }

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger())
            return false;

        var targets = RangeDetectionUtility.GetAttackTargets(transform.position, stunRange, default, targetLayer);

        if (targets.Count == 0)
            return false;

        foreach (var t in targets)
        {
            if (t.TryGetComponent(out Status status))
            {
                StatusEffect stun = new SlowDown(t.gameObject, 100f, Data.GetValue(1));
                StatusEffectManager.Instance.AddStatusEffect(status, stun);
            }
        }

        return true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffect slow = new SlowDown(other.gameObject, Data.GetValue(0));
            StatusEffectManager.Instance.AddStatusEffect(status, slow);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Status status))
        {
            StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(SlowDown));
        }
    }
}