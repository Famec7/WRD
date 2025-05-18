using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

public class BoobyTrap : ClickTypeSkill
{
    float effectTime;

    protected override void Init()
    {
        base.Init();

        effectTime = Data.GetValue(0);
    }

    public override void OnActiveEnter()
    {
        ParticleEffect particleEffect = EffectManager.Instance.CreateEffect<ParticleEffect>("BoobyTrap");
        particleEffect.SetPosition(ClickPosition);
        GameObject particleEffectTrap = particleEffect.gameObject.transform.GetChild(0).gameObject;
        particleEffectTrap.GetComponent<SlowZone>().SetData(effectTime, Data.Range, Data.GetValue(2));
        particleEffectTrap.GetComponent<DamageAmplificationZone>().SetData(effectTime, Data.Range, Data.GetValue(1));
        particleEffectTrap.transform.SetParent(null);
        particleEffectTrap.GetComponent<SlowZone>().SetPosition(ClickPosition);
        particleEffectTrap.GetComponent<SlowZone>().PlayEffect();
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {

    }
}
