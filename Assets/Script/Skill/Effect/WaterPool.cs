using System;
using System.Collections;
using UnityEngine;

public class WaterPool : MonoBehaviour
{
    [Header("물웅덩이 크기")]
    [SerializeField] private float _poolRadius = 2.0f;
    
    [SerializeField]
    private SlowZone _slowZone;
    [SerializeField]
    private DamageAmplificationZone _damageAmplificationZone;

    private WaitForSeconds _duration;

    public void Init(float slowRate, float damageAmplificationRate, float duration)
    {
        _slowZone.SetData(0, _poolRadius, slowRate);
        _damageAmplificationZone.SetData(0, _poolRadius, damageAmplificationRate);
        
        Vector3 newScale = new Vector3(_poolRadius, _poolRadius, 1);
        this.transform.localScale = newScale;
        
        _duration = new WaitForSeconds(duration);
    }

    public void PlayEffect()
    {
        this.gameObject.SetActive(true);
        
        _slowZone.SetPosition(this.transform.position);
        _slowZone.PlayEffect();
        
        _damageAmplificationZone.SetPosition(this.transform.position);
        _damageAmplificationZone.PlayEffect();
        
        StartCoroutine(IE_PlayEffect());
    }

    public void StopEffect()
    {
        this.gameObject.SetActive(false);
        
        _slowZone.StopEffect();
        _damageAmplificationZone.StopEffect();
    }
    
    private IEnumerator IE_PlayEffect()
    {
        yield return _duration;
        
        StopEffect();
    }
}