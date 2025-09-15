using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsMenu : MonoBehaviour
{
    public GameObject menuPanel; // assign your Panel here
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind the action to toggle the menu
        controls.UI.OptionsMenu.performed += ctx =>
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        };
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
