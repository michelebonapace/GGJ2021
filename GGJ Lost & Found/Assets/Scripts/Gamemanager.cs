using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : Singleton<Gamemanager>
{
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public float gameTime;

    private Player player;

    private int totalScore = 0;
    private float gameTimer = 0f;
    private bool hasGameStarted = false;

    public Player Player { get { if (player == null) player = FindObjectOfType<Player>(); return player; } }
    public float GetCurrentGameTimer { get => gameTimer; }

    public override void Awake()
    {
        dontDestroy = true;

        base.Awake();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (hasGameStarted && gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
        }
        else if (hasGameStarted)
        {
            hasGameStarted = false;

            OnGameEnd?.Invoke();
        }
    }

    public (float, float) GetRemainingTime()
    {
        float minutes = Mathf.FloorToInt(gameTimer / 60);
        float seconds = Mathf.FloorToInt(gameTimer % 60);

        return (minutes, seconds);
    }

    public int GetCurrentScore()
    {
        return totalScore;
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Player != null)
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        gameTimer = gameTime;
        hasGameStarted = true;

        OnGameStart?.Invoke();
    }
}
