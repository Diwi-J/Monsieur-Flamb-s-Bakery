using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit; //Seconds for time limit
    private float currentTime;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameOverMenu gameOverMenu;

    [Header("Flash Settings")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float flashThreshold = 10f; //Seconds left when it flashes

    private bool isGameOver = false;

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0)
        {
            currentTime = 0;
            gameOverMenu.GameOver();
            isGameOver = true;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //Flash red when reaching a set time
        if (currentTime <= flashThreshold)
        {
            float t = Mathf.PingPong(Time.time * 5, 1f);
            timerText.color = Color.Lerp(normalColor, warningColor, t);
        }
        else
        {
            timerText.color = normalColor;
        }
    }

    public void ObjectiveComplete()
    {
        isGameOver = true;

        Debug.Log("Objective completed in time!");
    }
}
