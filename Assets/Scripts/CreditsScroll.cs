using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 20f;        // Normal scroll speed
    public float fastScrollSpeed = 60f;    // Speed when space is held
    public float stopTimeAfterCredits = 2f; // Seconds to wait before loading menu
    public string mainMenuSceneName = "MainMenu"; // Scene to return to

    public AudioClip bgm;

    private AudioSource audioSource;
    public float endYPosition = 1000f; // Y position at which credits are considered "finished"

    private bool creditsFinished = false;

    void Start()
    {
        // Create an AudioSource dynamically if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgm;
       

        // Play BGM
        if (bgm != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        if (!creditsFinished)
        {
            // Check if space is held for fast scrolling
            float currentSpeed = Input.GetKey(KeyCode.Space) ? fastScrollSpeed : scrollSpeed;

            // Move credits upward in local space
            transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

            // If credits pass the end Y position, stop
            if (transform.position.y >= endYPosition)
            {
                creditsFinished = true;
                StartCoroutine(ReturnToMenuAfterDelay());
            }
        }
    }

    private System.Collections.IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(stopTimeAfterCredits);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}