using UnityEngine;

public class TempDebugFRNT : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    void OnEnable() => Debug.Log("[NametagDebugger] Enabled at time " + Time.time);
    void OnDisable() => Debug.Log("[NametagDebugger] Disabled at time " + Time.time);
    void OnTransformParentChanged() => Debug.Log("[NametagDebugger] Parent changed to: " + transform.parent?.name);
    void OnTransformChildrenChanged() => Debug.Log("[NametagDebugger] Children changed");
    void Update()
    {
        // show if sorting order was modified
        if (canvas != null && canvas.sortingOrder != -999) // change -999 to initial if needed
        {
            // optional, only when changed you can store last value and print
        }
    }

    CanvasGroup cg;
    void Start()
    {
        GameObject bg = new GameObject("DebugBG", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(transform, false);
        RectTransform rt = bg.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        Image img = bg.GetComponent<Image>();
        img.color = Color.magenta; // semi-transparent
    }
}

}


