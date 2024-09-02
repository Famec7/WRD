using System;
using System.Collections;
using UnityEngine;

public class PopUpSlide : MonoBehaviour
{
    private Vector3 originalPosition;

    [Header("PopUp Slide Position")] [SerializeField]
    private Vector3 popUpPosition;

    private RectTransform _currentRectTransform;

    private bool _isPopUp = false;
    
    [SerializeField]
    private AnimationCurve _animationCurve;

    private void Awake()
    {
        _currentRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalPosition = _currentRectTransform.anchoredPosition;
    }

    public void OnClick()
    {
        StartCoroutine(_isPopUp ? IE_ClosePopUp() : IE_OpenPopUp());
    }

    private IEnumerator IE_OpenPopUp()
    {
        _isPopUp = true;
        
        float duration = 1.0f;
        float elapsedTime = 0.0f;

        while (!IsTransformEqual(popUpPosition))
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = _animationCurve.Evaluate(t);

            _currentRectTransform.anchoredPosition =
                Vector3.Lerp(_currentRectTransform.anchoredPosition, popUpPosition, t);

            yield return null;
        }
    }

    private IEnumerator IE_ClosePopUp()
    {
        _isPopUp = false;
        
        float duration = 1.0f;
        float elapsedTime = 0.0f;
        
        while (!IsTransformEqual(originalPosition))
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = _animationCurve.Evaluate(t);

            _currentRectTransform.anchoredPosition =
                Vector3.Lerp(_currentRectTransform.anchoredPosition, originalPosition, t);

            yield return null;
        }
    }

    private bool IsTransformEqual(Vector3 compare, float threshold = 0.01f)
    {
        return Vector3.Distance(_currentRectTransform.anchoredPosition, compare) < threshold;
    }
}