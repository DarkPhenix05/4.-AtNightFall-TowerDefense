using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public string _levelToLoad;
    public void Play()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(_levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
