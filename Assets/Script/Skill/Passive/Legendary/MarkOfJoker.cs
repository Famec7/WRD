using UnityEngine;

public class MarkOfJoker : PassiveSkillBase
{
    [SerializeField] private float _range = 1.0f;
    [SerializeField] private AudioClip sfx;
    
    public override bool Activate(GameObject target = null)
    {
        if(!CheckTrigger())
            return false;
        
        if (target is null)
            return false;

        if (target.TryGetComponent(out Status status))
        {
            if (sfx != null)
            {
                SoundManager.Instance.PlaySFX(sfx);
            }
            
            StatusEffectManager.Instance.AddStatusEffect(status, new JokerMark(status.gameObject, OnEndDebuff));
            
            return true;
        }

        return false;
    }
    
    private void OnEndDebuff(Monster monster)
    {
        var targets = RangeDetectionUtility.GetAttackTargets(monster.transform.position, _range, default, targetLayer);

        foreach (var target in targets)
        {
            if (target.TryGetComponent(out Monster targetMonster))
            {
                targetMonster.HasAttacked(Data.GetValue(0));
            }
        }
    }
}