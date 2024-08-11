using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LayerMaskManager :Singleton<LayerMaskManager>
{
    [SerializeField]
    private LayerMask _monsterLayerMask;
    
    public LayerMask MonsterLayerMask => _monsterLayerMask;

    protected override void Init()
    {
        ;
    }
}