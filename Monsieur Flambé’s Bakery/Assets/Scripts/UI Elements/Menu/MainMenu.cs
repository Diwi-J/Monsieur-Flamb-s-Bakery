using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Load the main game scene.
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        //Quit the Game.
        Application.Quit();

        Debug.Log("Application Quit");
    }
}
