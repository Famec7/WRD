using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSlide : MonoBehaviour
{
    protected Vector3 originalPosition;

    [Header("PopUp Slide Position")] [SerializeField]
    protected Vector3 popUpPosition;

    protected RectTransform _currentRectTransform;
    protected bool _isPopUp = false;
    [SerializeField]
    protected Sprite rightImage;
    [SerializeField]
    protected Sprite leftImage;
    [SerializeField]
    protected Image openButton;

    [SerializeField]
    protected AnimationCurve _animationCurve;

    protected virtual void Awake()
    {
        _currentRectTransform = GetComponent<RectTransform>();
    }

    protected void Start()
    {
        originalPosition = _currentRectTransform.anchoredPosition;
    }

    public virtual void OnClick()
    {
        StartCoroutine(_isPopUp ? IE_ClosePopUp() : IE_OpenPopUp());
        openButton.sprite = _isPopUp ? leftImage : rightImage ;
    }

    protected virtual IEnumerator IE_OpenPopUp()
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

    protected virtual IEnumerator IE_ClosePopUp()
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

    protected bool IsTransformEqual(Vector3 compare, float threshold = 0.01f)
    {
        return Vector3.Distance(_currentRectTransform.anchoredPosition, compare) < threshold;
    }


}