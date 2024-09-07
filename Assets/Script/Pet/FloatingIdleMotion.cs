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
    
    private Vector3 _startPos;
    
    private void Start()
    {
        _startPos = transform.position;
        StartCoroutine(IE_FloatingIdle());
    }
    
    private IEnumerator IE_FloatingIdle()
    {
        while (true)
        {
            float dy = Mathf.Sin(Time.time * _floatingSpeed) * _floatingHeight;
            transform.position += new Vector3(0, dy, 0);
            
            yield return null;
        }
    }
}