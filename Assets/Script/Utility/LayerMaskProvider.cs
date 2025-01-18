using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class LayerMaskProvider
{
    public static LayerMask MonsterLayerMask { get; private set; }

    static LayerMaskProvider()
    {
        MonsterLayerMask = LayerMask.GetMask("Monster");
    }
}