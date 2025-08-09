using UnityEngine;

public class MenuStuff : MonoBehaviour
{

    public AudioClip bgm;

    public AudioSource audioSource;

    void Start()
    {
        // Create an AudioSource dynamically if not already present
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = bgm;


        // Play BGM
        if (bgm != null)
        {
            audioSource.Play();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Prologue");

    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void Credits()
    {
        Debug.Log("Quit Game");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
}
