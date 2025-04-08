using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoobyTrapVariation : PassiveSkillBase
{
    [SerializeField]
    private GameObject _trap;
    float effectTime;
    protected override void Init()
    {
        base.Init();
        effectTime = Data.GetValue(0);
    }
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger()) return false;
        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>("BoobyTrap");
        particleEffect.SetPosition(target.transform.position);
        GameObject particleEffectTrap = particleEffect.gameObject.transform.GetChild(0).gameObject;
        particleEffectTrap.GetComponent<SlowZone>().SetData(effectTime, Data.Range, Data.GetValue(2));
        particleEffectTrap.GetComponent<DamageAmplificationZone>().SetData(effectTime, Data.Range, Data.GetValue(1));
        particleEffectTrap.transform.SetParent(null);
        particleEffectTrap.GetComponent<SlowZone>().SetPosition(target.transform.position);
        particleEffectTrap.GetComponent<SlowZone>().PlayEffect();
        return true;
    }
}
