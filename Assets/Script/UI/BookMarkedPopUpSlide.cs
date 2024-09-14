using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BookMarkedPopUpSlide : PopUpSlide
{
    private WeaponImage[] WeaponImages;
    public bool isSliding = true;
    protected override void Awake()
    {
        base.Awake();
        WeaponImages = GetComponentsInChildren<WeaponImage>();
    }
    public override void OnClick()
    {
        StartCoroutine(_isPopUp ? IE_ClosePopUp() : IE_OpenPopUp());
        openButton.sprite = _isPopUp ? leftImage : rightImage;
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

            foreach (var weaponImage in WeaponImages)
            {
                weaponImage.transform.localPosition = Vector3.zero;
            }

            yield return null;
        }
        OnPopUpComplete();
    }
    protected override IEnumerator IE_OpenPopUp()
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

            foreach (var weaponImage in WeaponImages)
            {
                weaponImage.transform.localPosition = Vector3.zero;
            }

            yield return null;
        }
        OnPopUpComplete();
    }
    protected void OnPopUpComplete()
    {
        foreach (var weaponImage in WeaponImages)
            weaponImage.isSlide = true;

        foreach (var weaponImage in WeaponImages)
        {
            RectTransform rectTransform = weaponImage.GetComponent<RectTransform>();

            if (rectTransform != null)
                rectTransform.anchoredPosition = Vector2.zero; 
            else
                weaponImage.transform.localPosition = Vector3.zero; 

            weaponImage.originalPos = weaponImage.transform.parent.position;
            weaponImage.isSlide = false;
        }
    }
}

