using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject pauseMenuUI;       // whole pause menu canvas
    public GameObject pauseMenu;         // main pause panel
    public GameObject settingsMenu;      // settings panel
    public GameObject recipeCanvas;      // recipe canvas

    [Header("Player")]
    public PlayerController playerController;

    [Header("Input System")]
    public InputAction pauseAction;      // assign in inspector

    [Header("First Selected Buttons")]
    public GameObject firstPauseButton;      // first button in main pause menu
    public GameObject firstSettingsButton;   // first button in settings menu

    [Header("Recipe Panel")]
    public GameObject recipePanel;       // optional

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
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        if (recipeCanvas != null)
            recipeCanvas.SetActive(false);

        Time.timeScale = 0f;
        IsPaused = true;

        if (playerController != null)
            playerController.enabled = false;

        SelectButton(firstPauseButton);
    }

    public void ResumeGame()
    {
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
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        if (recipePanel != null)
            recipePanel.SetActive(false);

        SelectButton(firstSettingsButton);
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        if (recipePanel != null)
            recipePanel.SetActive(true);

        SelectButton(firstPauseButton);
    }

    // Utility method to handle EventSystem selection
    private void SelectButton(GameObject button)
    {
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}
