using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ThunderStrikeEffect : EffectBase
{
    private VisualEffect _visualEffect = null;
    
    private WaitForSeconds _delay = null;
    
    protected override void Init()
    {
        _visualEffect = GetComponent<VisualEffect>();
        _delay = new WaitForSeconds(0.1f);
    }

    public override void PlayEffect()
    {
        _visualEffect.Play();
        StartCoroutine(IE_WaitForStop());
    }

    public override void StopEffect()
    {
        _visualEffect.Stop();
        EffectManager.Instance.ReturnEffectToPool(this, "ThunderStrike");
    }
    
    private IEnumerator IE_WaitForStop()
    {
        yield return _delay;
        
        while (_visualEffect.HasAnySystemAwake())
        {
            yield return null;
        }
        
        StopEffect();
    }
}