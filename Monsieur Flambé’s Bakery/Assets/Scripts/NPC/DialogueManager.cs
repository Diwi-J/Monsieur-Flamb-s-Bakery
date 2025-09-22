using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text dialogueText;

    private string[] sentences;
    private int index;

    private PlayerControls controls;

    void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(false);

        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.NextDialogue.performed += OnNextPressed;
    }

    void OnDisable()
    {
        controls.Player.NextDialogue.performed -= OnNextPressed;
        controls.Player.Disable();
    }


    //Handle input to continue dialogue
    private void OnNextPressed(InputAction.CallbackContext context)
    {
        if (dialogueUI.activeSelf) //Only continue if dialogue is open
            NextSentence();
    }


    //Start a new dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueUI.SetActive(true);
        nameText.text = dialogue.npcName;

        sentences = dialogue.sentences;
        index = 0;
        dialogueText.text = sentences[index];
    }


    //Show next sentence or end dialogue if finished
    public void NextSentence()
    {
        index++;
        if (index < sentences.Length)
            dialogueText.text = sentences[index];
        else
            EndDialogue();
    }


    //End the dialogue and hide UI
    void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
}
