using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerInteractable playerInteractable;

    private void Awake()
    {
        controls = new PlayerControls();
        playerInteractable = GetComponent<PlayerInteractable>();

        controls.Player.Interact.performed += ctx => playerInteractable.TryPickUp();
        controls.Player.Drop.performed += ctx => playerInteractable.DropItem();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
