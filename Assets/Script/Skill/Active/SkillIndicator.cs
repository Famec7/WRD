using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillIndicator : MonoBehaviour
{
    private ActiveSkillBase _skill;
    
    public void SetSkill(ActiveSkillBase skill)
    {
        _skill = skill;
    }
    
    public void ShowIndicator(Vector3 position = default)
    {
        transform.position = position;
    }
    
    public void HideIndicator()
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
        if (other.CompareTag("Monster"))
        {
            if(other.TryGetComponent(out Monster monster))
            {
                _skill.AddTargetMonster(monster);
            }
        }
    }
}