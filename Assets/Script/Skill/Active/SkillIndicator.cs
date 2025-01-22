using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillIndicator : MonoBehaviour
{
    private Collider2D _collider;
    private ActiveSkillBase _skill;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private bool _isFixedPosition = false;

    public void SetSkill(ActiveSkillBase skill)
    {
        _skill = skill;

        Vector3 newScale = default;
        switch (skill.IndicatorType)
        {
            case IndicatorManager.Type.Circle:
                newScale = new Vector3(skill.Data.Range, skill.Data.Range, 1);
                break;
            case IndicatorManager.Type.Triangle:
                newScale = new Vector3(skill.Data.Range, skill.Data.Range, 1);
                break;
            case IndicatorManager.Type.Square:
                newScale = new Vector3(skill.Data.Range, 1, 1);
                break;
            default:
                break;
        }
        
        this.transform.localScale = newScale;
    }

    public virtual void ShowIndicator(Vector3 position = default, bool isRender = true)
    {
        _spriteRenderer.enabled = isRender;
        transform.position = position;
    }

    public virtual void HideIndicator()
    {
        _spriteRenderer.enabled = false;
    }
    
    public Collider2D Collider => _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }
}