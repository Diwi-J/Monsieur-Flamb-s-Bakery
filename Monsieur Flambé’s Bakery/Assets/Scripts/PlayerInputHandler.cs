using UnityEngine;

[RequireComponent(typeof(PlayerInteractable))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerInteractable playerInteractable;

    private void Awake()
    {
        //Initialize PlayerControls and PlayerInteractable.
        controls = new PlayerControls();
        playerInteractable = GetComponent<PlayerInteractable>();

        controls.Player.Interact.performed += ctx => playerInteractable.TryInteract();
        controls.Player.Drop.performed += ctx => playerInteractable.DropItem();
    }

    //Enable and Disable the controls when the script is enabled or disabled.
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}

