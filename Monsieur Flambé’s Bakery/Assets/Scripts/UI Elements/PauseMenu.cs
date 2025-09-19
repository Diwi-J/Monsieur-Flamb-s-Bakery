/*using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenuUI;      //This is the Whole Canvas
    GameObject pauseMenu;        //This is only the PauseCanvas
    GameObject settingsMenu;     //This is only the SettingsCanvas

    public PlayerController movement;

    bool IsPaused = false;

    private void Awake()
    {
        pauseMenuUI = gameObject;

        pauseMenu = pauseMenuUI.transform.GetChild(0).gameObject;
        settingsMenu = pauseMenuUI.transform.GetChild(1).gameObject;

    }
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseMenu.SetActive(false);   
        settingsMenu.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        Time.timeScale = 0f;
        IsPaused = true;

        PlayerController.OnDisable();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;

        PlayerController.OnEnable();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Application Quit");
    }

    public void SettingsMenu()
    {
        pauseMenuUI.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Back()
    {
        pauseMenuUI.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}*/
