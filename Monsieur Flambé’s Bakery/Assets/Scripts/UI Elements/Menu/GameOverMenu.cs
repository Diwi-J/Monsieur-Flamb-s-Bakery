using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    GameObject gameOverMenuUI;      //This is the Whole Canvas
    GameObject gameOverMenu;        //This is Only the GameOverCanvas

    public PlayerController playerController;

    public void Awake()
    {
        gameOverMenuUI = gameObject;

        gameOverMenu = gameOverMenuUI.transform.GetChild(0).gameObject;
    }

    public void Start()
    {
        gameOverMenuUI.SetActive(false);   
        gameOverMenu.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        gameOverMenuUI.SetActive(true);
        gameOverMenu.SetActive(true);

        playerController.OnDisable();

        Debug.Log("Game Over");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);

        gameOverMenuUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Application Quit");
    }
}
