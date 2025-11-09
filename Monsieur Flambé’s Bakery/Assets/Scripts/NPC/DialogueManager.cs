using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TMPro.TMP_Text nameText;
    public TMPro.TMP_Text dialogueText;

    private string[] sentences;
    private int index;

    [Header("Particles (optional)")]
    public ParticleSystem celebrationParticles;
    public Transform celebrationSpawnPoint;

    private PlayerControls controls;

    //Track if this dialogue should trigger confetti.
    private bool triggerCelebration = false;

    public Dialogue LastDialogue { get; private set; } //Stores last dialogue.
    public event Action onDialogueComplete;

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

    private void OnNextPressed(InputAction.CallbackContext context)
    {
        if (dialogueUI.activeSelf)
            NextSentence();
    }

    public void StartDialogue(Dialogue dialogue, bool isCelebration = false, Transform celebrationPoint = null)
    {
        if (dialogue == null) return;

        LastDialogue = dialogue;

        dialogueUI.SetActive(true);
        nameText.text = dialogue.npcName;

        sentences = dialogue.sentences;
        index = 0;
        dialogueText.text = sentences[index];

        triggerCelebration = isCelebration;
        celebrationSpawnPoint = celebrationPoint;
    }

    public void NextSentence()
    {
        index++;
        if (index < sentences.Length)
            dialogueText.text = sentences[index];
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);

        //Celebration particles.
        if (triggerCelebration && celebrationParticles != null && celebrationSpawnPoint != null)
        {
            Vector3 spawnPos = celebrationSpawnPoint.position + Vector3.up * 1.5f;
            Quaternion upright = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

            ParticleSystem ps = Instantiate(celebrationParticles, spawnPos, upright);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        //Fire completion event.
        onDialogueComplete?.Invoke();

        triggerCelebration = false;
        celebrationSpawnPoint = null;
    }
}
