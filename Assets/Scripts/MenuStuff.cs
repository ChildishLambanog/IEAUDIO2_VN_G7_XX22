using UnityEngine;

public class MenuStuff : MonoBehaviour
{
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
