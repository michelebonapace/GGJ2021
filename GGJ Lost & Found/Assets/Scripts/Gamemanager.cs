using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public float gameTime;

    private PlayerController player;

    private float gameTimer;
    private bool hasGameStarted = false;

    public PlayerController Player { get { if (player == null) player = FindObjectOfType<PlayerController>(); return player; } }
    public float GetCurrentGameTimer { get => gameTimer; }

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

    public void StartNewGame()
    {
        gameTimer = gameTime;
        hasGameStarted = true;

        OnGameStart?.Invoke();
    }
}
