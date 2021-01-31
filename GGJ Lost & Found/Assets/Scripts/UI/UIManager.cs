using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;

    public delegate (float minutes, float seconds) TimerDelegate();
    private TimerDelegate timerTextDelegate;

    public delegate int ScoreDelegate();
    private ScoreDelegate scoreTextDelegate;

    private void Start()
    {
        timerTextDelegate = Gamemanager.Instance.GetRemainingTime;
        scoreTextDelegate = Gamemanager.Instance.GetCurrentScore;
    }

    private void Update()
    {
        UpdateTimerText();
        UpdateScoreText();
    }

    public void UpdateTimerText()
    {
        var timerTextData = timerTextDelegate();
        timerText.text = string.Format("{0:00}:{1:00}", timerTextData.minutes, timerTextData.seconds);
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + Gamemanager.Instance.GetCurrentScore();
    }
}
