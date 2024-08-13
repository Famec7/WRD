using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillIndicator : MonoBehaviour
{
    private ActiveSkillBase _skill;
    private CircleCollider2D _collider;

    public void SetSkill(ActiveSkillBase skill)
    {
        _skill = skill;

        this.transform.localScale = new Vector3(skill.Data.Range, skill.Data.Range, 0);
    }

    public void ShowIndicator(Vector3 position = default)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

    public void HideIndicator()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Monster monster))
        {
            _skill.AddTargetMonster(monster);
        }
    }
}