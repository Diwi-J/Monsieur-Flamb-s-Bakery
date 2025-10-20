using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private GameObject gameOverMenuUI;   // Whole Canvas
    private GameObject gameOverMenu;     // Inner GameOver UI Panel

    public PlayerController playerController;

    private void Awake()
    {
        gameOverMenuUI = gameObject;
        gameOverMenu = gameOverMenuUI.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        gameOverMenuUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void GameOver()
    {
        // Pause game
        Time.timeScale = 0f;

        // Show UI
        gameOverMenuUI.SetActive(true);
        gameOverMenu.SetActive(true);

        // Disable player movement the proper way
        if (playerController != null)
            playerController.enabled = false;

        Debug.Log("Game Over");
    }

    public void Restart()
    {
        // Unpause time
        Time.timeScale = 1f;

        // Reactivate player (optional — scene will reload anyway)
        if (playerController != null)
            playerController.enabled = true;

        // Start a coroutine to reload scene safely
        StartCoroutine(RestartRoutine());
    }

    private System.Collections.IEnumerator RestartRoutine()
    {
        // Load scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until it’s done
        while (!asyncLoad.isDone)
            yield return null;

        // Now the new scene is active
        // Reset UI just in case
        gameOverMenuUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
