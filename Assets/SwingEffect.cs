using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingEffect : MonoBehaviour
{
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        StopEffect();
    }
    
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void PlayEffect()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(IE_PlayEffect());
    }

    public void StopEffect()
    {
        this.gameObject.SetActive(false);
        _material.SetFloat("_value", 0);
    }

    private IEnumerator IE_PlayEffect()
    {
        while (_material.GetFloat("_value") < 1)
        {
            _material.SetFloat("_value", _material.GetFloat("_value") + Time.deltaTime * 5);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        StopEffect();
    }
}