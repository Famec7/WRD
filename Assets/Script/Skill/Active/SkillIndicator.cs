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
    }

    public virtual void ShowIndicator(Vector3 position = default)
    {
        _spriteRenderer.enabled = true;

        if (_isFixedPosition)
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
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.AddTargetMonster(monster);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.AddTargetMonster(monster);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.RemoveTargetMonster(monster);
        }
    }
}