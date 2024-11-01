using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillIndicator : MonoBehaviour
{
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

    public virtual void ShowIndicator(Vector3 position = default, bool isFixedPosition = false)
    {
        _spriteRenderer.enabled = true;

        if (isFixedPosition)
        {
            return;
        }

        transform.position = position;
    }

    public virtual void HideIndicator()
    {
        _spriteRenderer.enabled = false;
    }

    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_skill is null)
        {
            return;
        }
        
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.AddTargetMonster(monster);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_skill is null)
        {
            return;
        }
        
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.AddTargetMonster(monster);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_skill is null)
        {
            return;
        }
        
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.RemoveTargetMonster(monster);
        }
    }
}