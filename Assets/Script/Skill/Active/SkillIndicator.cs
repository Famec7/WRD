using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillIndicator : MonoBehaviour
{
    private ActiveSkillBase _skill;

    [SerializeField] private bool _isFixedPosition = false;

    public void SetSkill(ActiveSkillBase skill)
    {
        _skill = skill;
    }

    public virtual void ShowIndicator(Vector3 position = default)
    {
        gameObject.SetActive(true);

        if (_isFixedPosition)
        {
            return;
        }

        transform.position = position;
    }

    public virtual void HideIndicator()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.RemoveTargetMonster(monster);
        }
    }
}