using UnityEngine;

public class SkySwordEffect : EffectBase
{
    protected override void Init()
    {
        ;
    }

    public override void PlayEffect()
    {
        ;
    }

    public override void StopEffect()
    {
        EffectManager.Instance.ReturnEffectToPool(this, "SkySwordAura");
    }
}