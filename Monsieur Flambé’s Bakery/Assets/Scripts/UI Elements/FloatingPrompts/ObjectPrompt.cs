using UnityEngine;

public class ObjectPrompt : MonoBehaviour
{
    [Header("Prompt Settings")]
    public string promptMessage = "Press E";
    public float displayDistance = 3f;

    private Transform player;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogWarning("[ObjectPrompt] Player not found. Tag your player 'Player'.");
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // Only show if FloatingPrompt exists
        if (FloatingPrompt.Instance != null)
        {
            if (dist <= displayDistance)
                FloatingPrompt.Instance.Show(promptMessage);
            else
                FloatingPrompt.Instance.Hide();
        }
    }

    private void OnDisable()
    {
        if (FloatingPrompt.Instance != null)
            FloatingPrompt.Instance.Hide();
    }
}
