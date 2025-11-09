using UnityEngine;
using UnityEngine.SceneManagement;

public class CelebrationMenu : MonoBehaviour
{
    public void PlayAgain()
    {
        Time.timeScale = 1f; //Unpause.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Load main menu.
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    //Quit the game.
    public void QuitGame()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
