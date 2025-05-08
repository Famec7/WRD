using System.Collections;
using UnityEngine;

public class LineRendererController : EffectBase
{
    private LineRenderer _lineRenderer;

    [SerializeField] private Texture[] _textures;
    [SerializeField] private float _fps = 30.0f;

    private int _animationStep = 0;
    private float _fpsTime = 0.0f;
    private Coroutine _coroutine;
    
    protected override void Init()
    {
        if (TryGetComponent<LineRenderer>(out _lineRenderer) == false)
        {
            Debug.LogError("LineRenderer 컴포넌트가 없습니다.");
            return;
        }
    }

    public void LinkTarget(Vector3 from, Vector3 to)
    {
        _lineRenderer.SetPosition(0, from);
        _lineRenderer.SetPosition(1, to);
    }

    public override void PlayEffect()
    {
        if (_coroutine == null)
        {
            this.gameObject.SetActive(true);
            _coroutine = StartCoroutine(IE_PlayEffect());
        }
    }

    public override void StopEffect()
    {
        if (_coroutine == null)
        {
            return;
        }

        this.gameObject.SetActive(false);
        StopCoroutine(_coroutine);
        _coroutine = null;
        
        _animationStep = 0;
        _fpsTime = 0.0f;
        
        EffectManager.Instance.ReturnEffectToPool(this, "ChainLightning");
    }
    
    private IEnumerator IE_PlayEffect()
    {
        while (_animationStep < _textures.Length)
        {
            _fpsTime += Time.deltaTime;

            if (_fpsTime >= 1.0f / _fps)
            {
                _animationStep++;
                
                if (_animationStep >= _textures.Length)
                {
                    break;
                }
                
                _lineRenderer.material.mainTexture = _textures[_animationStep];
                _fpsTime = 0.0f;
            }

            yield return null;
        }
        
        StopEffect();
    }
}