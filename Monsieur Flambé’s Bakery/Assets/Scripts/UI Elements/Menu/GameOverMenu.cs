using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private GameObject gameOverMenuUI;   //Whole Canvas.
    private GameObject gameOverMenu;     //Inner GameOver UI Panel.

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

    //Called when player loses (time ran out).
    public void GameOver()
    {
        ShowMenu("Game Over!");
    }

    //Called when player completes objective successfully.
    public void GameComplete()
    {
        ShowMenu("Objective Completed!");
    }

    //Unified menu display logic.
    private void ShowMenu(string title)
    {
        //Pause game.
        Time.timeScale = 0f;

        //Show UI.
        gameOverMenuUI.SetActive(true);
        gameOverMenu.SetActive(true);

        //Disable player movement.
        if (playerController != null)
            playerController.enabled = false;

        //Set a text field inside panel.
        var textField = gameOverMenu.GetComponentInChildren<UnityEngine.UI.Text>();
        if (textField != null)
            textField.text = title;

        Debug.Log(title);
    }

    public void Restart()
    {
        //Unpause time.
        Time.timeScale = 1f;

        //Reactivate player.
        if (playerController != null)
            playerController.enabled = true;

        //Reload scene safely.
        StartCoroutine(RestartRoutine());
    }

    private System.Collections.IEnumerator RestartRoutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
            yield return null;

        //Reset UI just in case.
        gameOverMenuUI.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void QuitGame()
    {
        //Quit Game.
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
