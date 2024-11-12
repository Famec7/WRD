using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiTrap : MonoBehaviour
{
    
    private List<Monster>_monsters = new List<Monster>();
    public float _damageAmplification;

    public void Init(float damageAmplification)
    {
        _damageAmplification = damageAmplification;
        _monsters.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            if (!_monsters.Contains(monster))
            {
                StatusEffect amplification = new DamageAmplification(monster.gameObject, _damageAmplification, 1);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, amplification);


                StatusEffect Stun = new SlowDown(monster.gameObject, 100, 0.5f);
                StatusEffectManager.Instance.AddStatusEffect(monster.status, Stun);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            if (_monsters.Contains(monster))
            {
                StatusEffectManager.Instance.RemoveStatusEffect(monster.status, typeof(DamageAmplification));
                _monsters.Remove(monster);   
            }
        }
    }
}
