using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BoobyTrapVariation : PassiveSkillBase
{
    private float _effectTime;
    private float _slowRate;
    private float _damageAmplificationRate;
    
    protected override void Init()
    {
        base.Init();
        _effectTime = Data.GetValue(0);
        _damageAmplificationRate = Data.GetValue(1);
        _slowRate = Data.GetValue(2);
    }
    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null)
        {
            return false;
        }
        
        BoobyTrapEffect particleEffect = EffectManager.Instance.CreateEffect<BoobyTrapEffect>("BoobyTrap");
        
        particleEffect.SetPosition(target.transform.position);
        particleEffect.SetData(_effectTime, Data.Range, _damageAmplificationRate, _slowRate);
        particleEffect.PlayDebuff();
        
        return true;
    }
}
