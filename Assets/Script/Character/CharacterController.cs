using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CharacterController : MonoBehaviour
{
    /***********************Variables*****************************/
    #region Data

    [SerializeField]
    private CharacterData _data;
    public CharacterData Data => _data;

    #endregion

    #region Target

    public virtual GameObject Target { get; set; }

    #endregion

    protected virtual void Awake()
    {
        Data.spriteRenderer = GetComponent<SpriteRenderer>();
    }
}