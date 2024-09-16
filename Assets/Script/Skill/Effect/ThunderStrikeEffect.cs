using System.Collections;
using UnityEngine.VFX;

public class ThunderStrikeEffect : EffectBase
{
    private VisualEffect _visualEffect = null;
    
    protected override void Init()
    {
        _visualEffect = GetComponent<VisualEffect>();
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
        yield return null;
        
        while (_visualEffect.HasAnySystemAwake())
        {
            yield return null;
        }
        
        StopEffect();
    }
}