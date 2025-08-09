using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsScroll : MonoBehaviour
{
    [Header("Scrolling")]
    public RectTransform creditsTransform;   // The content to scroll
    public float scrollSpeed = 50f;
    public float fastScrollMultiplier = 2f;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;      // Black fade overlay
    public float fadeDuration = 1f;

    [Header("End Settings")]
    public float endWaitTime = 2f;
    public string mainMenuSceneName = "MainMenu";

    private bool isFadingOut = false;
    private float startY;
    private float endY;
    private bool creditsFinished = false;

    void Start()
    {
        // Fade in from black
        fadeCanvasGroup.alpha = 1f;
        StartCoroutine(Fade(0f, fadeDuration));

        // Calculate start/end Y positions
        startY = creditsTransform.anchoredPosition.y;
        float viewportHeight = ((RectTransform)creditsTransform.parent).rect.height;
        endY = creditsTransform.sizeDelta.y - viewportHeight;
    }

    void Update()
    {
        if (creditsFinished || isFadingOut) return;

        float speed = scrollSpeed;
        if (Input.GetKey(KeyCode.Space))
        {
            speed *= fastScrollMultiplier;
        }

        creditsTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // Check if we reached the end
        if (creditsTransform.anchoredPosition.y >= endY)
        {
            creditsFinished = true;
            StartCoroutine(HandleCreditsEnd());
        }
    }

    private IEnumerator HandleCreditsEnd()
    {
        yield return new WaitForSeconds(endWaitTime);

        // Fade out
        isFadingOut = true;
        yield return Fade(1f, fadeDuration);

        // Load main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}