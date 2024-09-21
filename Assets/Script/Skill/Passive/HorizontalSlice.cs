using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSlice : Swing
{
    protected override ParticleEffect GetSwingEffect()
    {
        return EffectManager.Instance.CreateEffect<ParticleEffect>("HorizontalSliceEffect");
    }
}