using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // go to the game scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
