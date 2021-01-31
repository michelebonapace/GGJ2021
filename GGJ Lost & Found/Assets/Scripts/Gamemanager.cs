﻿using System;
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

    private float gameTimer;
    private bool hasGameStarted = false;

    public Player Player { get { if (player == null) player = FindObjectOfType<Player>(); return player; } }
    public float GetCurrentGameTimer { get => gameTimer; }

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
