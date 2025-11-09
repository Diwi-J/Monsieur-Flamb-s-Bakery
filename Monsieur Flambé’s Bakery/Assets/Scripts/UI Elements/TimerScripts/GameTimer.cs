using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 420f;
    private float currentTime;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Game Over UI")]
    public GameOverMenu gameOverMenu;

    [Header("Flash Settings")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float flashThreshold = 10f; //Seconds left.

    private bool isGameOver = false;    //Lose condition.
    private bool isStopped = false;     //Win condition.

    void Start()
    {
        //Initialize timer.
        currentTime = timeLimit;
    }

    void Update()
    {
        //Countdown logic.
        if (isGameOver || isStopped) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            TriggerGameOver();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        //Update timer text.
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        //Flash red near the end.
        if (!isStopped && currentTime <= flashThreshold)
        {
            float t = Mathf.PingPong(Time.time * 5, 1f);
            timerText.color = Color.Lerp(normalColor, warningColor, t);
        }
        else
        {
            timerText.color = normalColor;
        }

        //Turn green when objective completed.
        if (isStopped)
            timerText.color = Color.green;
    }

    private void TriggerGameOver()
    {
        //Handle game over state.
        isGameOver = true;

        if (gameOverMenu != null)
            gameOverMenu.GameOver();

        Debug.Log("Game Over! Timer ran out.");
    }

    //Call this when player completes the objective.
    public void StopTimerForObjective()
    {
        isStopped = true;
        Debug.Log("Timer stopped, objective completed!");
    }
}
