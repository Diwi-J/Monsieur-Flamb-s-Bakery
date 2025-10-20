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

    [Header("Particles")]
    public ParticleSystem celebrationParticles;      // Assign prefab in scene
    public Transform celebrationSpawnPoint;          // NPC or point above NPC

    private PlayerControls controls;

    // Track whether this is a "success" dialogue
    private bool triggerCelebration = false;

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

    // Handle input to continue dialogue
    private void OnNextPressed(InputAction.CallbackContext context)
    {
        if (dialogueUI.activeSelf) // Only continue if dialogue is open
            NextSentence();
    }

    // Start a new dialogue
    public void StartDialogue(Dialogue dialogue, bool isCelebration = false, Transform celebrationPoint = null)
    {
        dialogueUI.SetActive(true);
        nameText.text = dialogue.npcName;

        sentences = dialogue.sentences;
        index = 0;
        dialogueText.text = sentences[index];

        // Remember if this dialogue should trigger confetti
        triggerCelebration = isCelebration;

        // Assign spawn point for confetti
        celebrationSpawnPoint = celebrationPoint;
    }

    // Show next sentence or end dialogue if finished
    public void NextSentence()
    {
        index++;
        if (index < sentences.Length)
            dialogueText.text = sentences[index];
        else
            EndDialogue();
    }

    // End the dialogue and hide UI
    void EndDialogue()
    {
        dialogueUI.SetActive(false);

        // Spawn confetti only if this was a celebration dialogue
        if (triggerCelebration && celebrationParticles != null && celebrationSpawnPoint != null)
        {
            Vector3 spawnPos = celebrationSpawnPoint.position + Vector3.up * 1.5f;

            // Upright rotation with random Y for spread
            Quaternion upright = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            ParticleSystem ps = Instantiate(celebrationParticles, spawnPos, upright);
            ps.Play();

            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        triggerCelebration = false;
        celebrationSpawnPoint = null;
    }
}
