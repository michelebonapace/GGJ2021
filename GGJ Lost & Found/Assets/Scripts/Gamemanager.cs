using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : Singleton<Gamemanager>
{
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public AudioClip readyClip;
    public AudioClip goClip;

    public AudioClip[] endGameClips;

    public float gameTime;

    private Player player;
    private AudioSource audioSource;

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
            EndGame();
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

    public void AddTime(float amount)
    {
        gameTimer += amount;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Player != null)
        {
            StartCoroutine(GamePreparations());
        }
    }

    public void StartNewGame()
    {
        gameTimer = gameTime;
        hasGameStarted = true;

        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        hasGameStarted = false;

        PlayerPrefs.SetInt("Score", totalScore);

        audioSource.PlayOneShot(endGameClips[UnityEngine.Random.Range(0, endGameClips.Length)]);

        OnGameEnd?.Invoke();
        LoadScene(0);
    }

    private IEnumerator GamePreparations()
    {
        audioSource = Player.GetComponent<AudioSource>();
        Player.enabled = false;

        yield return new WaitForSeconds(1f);

        audioSource.PlayOneShot(readyClip);

        yield return new WaitForSeconds(0.5f);

        audioSource.PlayOneShot(goClip);
        player.enabled = true;

        StartNewGame();
    }
}
