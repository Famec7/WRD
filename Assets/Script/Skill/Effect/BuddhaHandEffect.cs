using UnityEngine;

public class BuddhaHandEffect : AnimationEffect
{
    [SerializeField]
    private ParticleSystem _effect;
    
    public void PlayGroundEffect()
    {
        if (_effect != null)
        {
            _effect.Play();
        }
    }

    public override void StopEffect()
    {
        if (_effect != null)
        {
            _effect.Stop();
        }
        
        gameObject.SetActive(false);
    }
}