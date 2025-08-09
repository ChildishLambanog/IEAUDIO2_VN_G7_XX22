using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RouteChoice : MonoBehaviour
{
    [Header("UI References")]
    public GameObject fadePanel;   // Fullscreen UI Panel (Image, black)
    private Image fadeImage;

    public Button button1;
    public Button button2;
    //public Button button3;

    [Header("Settings")]
    public float fadeDuration = 1f;
    public string scene1Name;
    public string scene2Name;
    //public string scene3Name;

    void Start()
    {
        fadeImage = fadePanel.GetComponent<Image>();

        // Start black
        fadePanel.SetActive(true);
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        // Fade in
        StartCoroutine(FadeIn());

        // Assign button actions
        button1.onClick.AddListener(() => StartCoroutine(FadeAndLoad(scene1Name)));
        button2.onClick.AddListener(() => StartCoroutine(FadeAndLoad(scene2Name)));
        //button3.onClick.AddListener(() => StartCoroutine(FadeAndLoad(scene3Name)));
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Fully transparent disable panel
        c.a = 0f;
        fadeImage.color = c;
        fadePanel.SetActive(false);
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        fadePanel.SetActive(true);

        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // Fully black  Load scene
        c.a = 1f;
        fadeImage.color = c;
        SceneManager.LoadScene(sceneName);
    }
}
