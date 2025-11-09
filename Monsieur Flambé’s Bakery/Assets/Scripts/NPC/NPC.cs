using UnityEngine;

public class NPC : Interactable
{
    [Header("Dialogue")]
    public Dialogue blockedDialogue;
    public Dialogue mainDialogue;

    [Header("Cake Target Zone")]
    public CakeTargetZone cakeZone;

    [Header("Door Unlock")]
    public HingeDoor linkedDoor;

    [Header("Win Screen (assign only on endgame NPC)")]
    public GameObject winPanel;  //Only this NPC triggers win screen.

    private bool unlocked = false;
    private bool winTriggered = false;

    public bool HasBeenSpokenTo => unlocked;

    public void UnlockDialogue()
    {
        unlocked = true;
    }

    public override void Interact()
    {
        unlocked = true;

        bool cakeInZone = cakeZone != null && cakeZone.IsCakePlaced();

        if (!cakeInZone && blockedDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(blockedDialogue, false);
        }
        else if (mainDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(mainDialogue, false);

            //Only subscribe if this NPC actually has a winPanel assigned.
            if (!winTriggered && winPanel != null)
                DialogueManager.Instance.onDialogueComplete += TriggerWinScreen;
        }

        if (linkedDoor != null)
        {
            linkedDoor.TryOpenDoor();
            Debug.Log("Door unlocked!");
        }
    }

    private void TriggerWinScreen()
    {
        //Only trigger once.
        winTriggered = true;

        //Unsubscribe immediately.
        DialogueManager.Instance.onDialogueComplete -= TriggerWinScreen;

        if (winPanel != null)
        {
            winPanel.SetActive(true);

            //Pause game.
            Time.timeScale = 0f;

            //Disable player movement.
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
                player.enabled = false;

            Debug.Log("Win screen triggered!");
        }
    }
}
