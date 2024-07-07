using System.Collections;
using UnityEngine;

public class VisionStrikeEffect : EffectBase
{
    private Material _material;
    
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
    }
    
    private IEnumerator IE_PlayEffect()
    {
        yield return new WaitForSeconds(0.5f);
        StopEffect();
    }
}