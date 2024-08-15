using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInShader : EffectBase
{
    private Material _material;
    private static readonly int Value = Shader.PropertyToID("_value");

    [SerializeField] private float _effectSpeed;

    protected override void Init()
    {
        _material = GetComponent<SpriteRenderer>().material;
        StopEffect();
    }

    public override void PlayEffect()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(IE_PlayEffect());
    }

    public override void StopEffect()
    {
        this.gameObject.SetActive(false);
        _material.SetFloat(Value, 0);
    }

    private IEnumerator IE_PlayEffect()
    {
        while (_material.GetFloat(Value) < 1)
        {
            _material.SetFloat(Value, _material.GetFloat(Value) + Time.deltaTime * _effectSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        StopEffect();
    }
}