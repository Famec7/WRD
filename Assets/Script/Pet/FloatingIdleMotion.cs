using System.Collections;
using UnityEngine;

public class FloatingIdleMotion : MonoBehaviour
{
    [Header("떠다니는 속도")]
    [SerializeField]
    private float _floatingSpeed = 1.0f;
    
    [Header("떠다니는 최고 높이")]
    [SerializeField]
    private float _floatingHeight = 0.5f;

    [Header("그림자")]
    [Space] [SerializeField] private Transform _shadow;
    [SerializeField] private float _minShadowScale = 0.01f;
    [SerializeField] private float _maxShadowScale = 0.02f;
    private Quaternion _originRotation;
    
    public void PlayFloatingIdle(Transform target)
    {
        _originRotation = _shadow.localRotation;
        StartCoroutine(IE_FloatingIdle(target));
    }
    
    public void StopFloatingIdle()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator IE_FloatingIdle(Transform target)
    {
        while (true)
        {
            float dy = Mathf.Sin(Time.time * _floatingSpeed) * _floatingHeight;
            target.localPosition = new Vector3(target.localPosition.x, dy, target.localPosition.z);
            
            float shadowScale = Mathf.Lerp(_maxShadowScale, _minShadowScale, (dy + _floatingHeight) / (2 * _floatingHeight));
            
            _shadow.localScale = new Vector3(shadowScale, shadowScale, 1);
            _shadow.localRotation = _originRotation;
            
            yield return null;
        }
        
        // ReSharper disable once IteratorNeverReturns
    }
    
    public void ShowShadow(bool show)
    {
        _shadow.gameObject.SetActive(show);
    }
}