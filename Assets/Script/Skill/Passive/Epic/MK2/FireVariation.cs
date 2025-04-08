using UnityEngine;

public class FireVariation : PassiveSkillBase
{
    [SerializeField]
    private AudioClip _fireSound;
    
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
        {
            return false;
        }

        var targets = RangeDetectionUtility.GetAttackTargets(target.transform.position, Data.Range, 360.0f, targetLayer);
        if (targets.Count == 0)
        {
            return true;
        }

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(Data.GetValue(0));

                if (!monster.CompareTag("Boss"))
                {
                    monster.HasAttackedCurrentPercent(Data.GetValue(1));
                }
            }
        }
        
        SoundManager.Instance.PlaySFX(_fireSound);

        return true;
    }
}