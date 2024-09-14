using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Message : MonoBehaviour
{
    public TextMeshProUGUI messageText; // 메시지를 표시할 Text 컴포넌트
    public float displayDuration = 2f; // 메시지가 표시되는 시간
    public float fadeDuration = 0.5f; // 페이드 아웃 시간

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(string text, float displayTime, float fadeTime)
    {
        messageText.text = text;
        displayDuration = displayTime;
        fadeDuration = fadeTime;
        canvasGroup.alpha = 1f;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(displayDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        // 메시지를 풀로 반환
        MessageManager.Instance.ReturnMessage(this);
    }
}
