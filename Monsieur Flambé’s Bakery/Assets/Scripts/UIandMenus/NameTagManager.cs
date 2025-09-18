using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameTagManager : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 5f;
    public float verticalOffset = 1.5f;

    private TextMeshProUGUI nameText;
    private GameObject nameGO;

    void Start()
    {
        if (playerCamera == null) playerCamera = Camera.main;

        // Create a canvas if none exists
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("NameTagCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        // Create text element
        nameGO = new GameObject("NameTagText");
        nameGO.transform.SetParent(canvas.transform, false);
        nameText = nameGO.AddComponent<TextMeshProUGUI>();
        nameText.fontSize = 36;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.raycastTarget = false;

        RectTransform rt = nameGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(400, 60);

        nameGO.SetActive(false);
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Raycast forward from camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Lookable lookable = hit.collider.GetComponent<Lookable>();
            if (lookable != null)
            {
                // Show text
                nameText.text = lookable.objectName;
                Vector3 worldPos = lookable.transform.position + Vector3.up * verticalOffset;
                Vector3 screenPos = playerCamera.WorldToScreenPoint(worldPos);

                if (screenPos.z > 0)
                {
                    nameGO.transform.position = screenPos;

                    float distance = Vector3.Distance(playerCamera.transform.position, lookable.transform.position);

                    if (!nameGO.activeSelf) nameGO.SetActive(true);
                }
                else
                {
                    nameGO.SetActive(false);
                }
                return;
            }
        }

        // Hide if not looking at anything with Lookable
        nameGO.SetActive(false);
    }
}
