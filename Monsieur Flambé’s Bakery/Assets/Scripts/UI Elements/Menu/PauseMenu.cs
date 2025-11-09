using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject pauseMenuUI;       //Whole pause menu canvas.
    public GameObject pauseMenu;         //Main pause panel.
    public GameObject settingsMenu;      //Settings panel.
    public GameObject recipeCanvas;      //Recipe canvas.
    
    [Header("Player")]
    public PlayerController playerController;

    [Header("Input System")]
    public InputAction pauseAction;      

    [Header("Recipe Panel")]
    public GameObject recipePanel;      

    private bool IsPaused = false;

    private void Awake()
    {
        if (pauseMenuUI == null)
            pauseMenuUI = gameObject;

        if (pauseMenu == null && pauseMenuUI.transform.childCount > 0)
            pauseMenu = pauseMenuUI.transform.GetChild(0).gameObject;

        if (settingsMenu == null && pauseMenuUI.transform.childCount > 1)
            settingsMenu = pauseMenuUI.transform.GetChild(1).gameObject;

        if (recipePanel == null)
            recipePanel = GameObject.Find("RecipePanel");
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    private void OnEnable() 
    {
        if (pauseAction != null)
        {
            pauseAction.Enable();
            pauseAction.performed += TogglePause;
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= TogglePause;
            pauseAction.Disable();
        }
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        //Toggle pause state.
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        //Show pause menu UI.
        pauseMenuUI.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        if (recipeCanvas != null)
            recipeCanvas.SetActive(false);

        Time.timeScale = 0f;
        IsPaused = true;

        if (playerController != null)
            playerController.enabled = false;

    }

    public void ResumeGame()
    {
        //Hide pause menu UI.
        pauseMenuUI.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);

        if (recipeCanvas != null)
            recipeCanvas.SetActive(true);

        Time.timeScale = 1f;
        IsPaused = false;

        if (playerController != null)
            playerController.enabled = true;
    }

    public void MainMenu()
    {
        //Return to main menu scene.
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        //Quit the Game.
        Application.Quit();
        Debug.Log("Application Quit");
    }

    public void OpenSettings()
    {
        //Open settings menu.
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        if (recipePanel != null)
            recipePanel.SetActive(false);

    }

    public void Back()
    {
        //Return to pause menu.
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        if (recipePanel != null)
            recipePanel.SetActive(true);

    }
}
