using UnityEngine;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    // === Game State ===
    public enum GameState { MainMenu, InGame, Paused, Shop, GameOver, LevelComplete }
    public GameState CurrentState { get; private set; }
    public int currentWave;

    // === Engine & Trailer Setup ===
    public EngineSO selectedEngineSO;
    public List<TrailerSO> selectedTrailerSOs;

    public EngineData engine;
    public List<TrailerData> trailers = new List<TrailerData>();

    // === Player Runtime Data ===
    public int playerMoney;
    public int resourceCount;
    public int enemiesKilledThisWave = 0;

    // === System References ===
//    public UIManager uiManager;
//    public EnemySpawner enemySpawner;
    public TrainMovement player;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        currentWave = 1;
        enemiesKilledThisWave = 0;

        engine = new EngineData(selectedEngineSO);
        trailers.Clear();

        foreach (var trailerSO in selectedTrailerSOs)
        {
            TrailerData trailer = new TrailerData(trailerSO);
            trailers.Add(trailer);
        }
        player.engine = engine;
        player.trailers = trailers;
        CalculatePlayerStats();
    }

    public void CalculatePlayerStats()
    {
        float totalHP = engine.maxHealth;
        float totalStorage = engine.baseStorage;

        foreach (var trailer in trailers)
        {
            totalHP += trailer.health;
            totalStorage += trailer.baseStats.baseStorage;
        }

        player.gameObject.GetComponent<Health>().maxHealth = totalHP;

        Debug.Log($"[GameHandler] Total HP: {totalHP}, Total Storage: {totalStorage}");
    }

/*    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        CurrentState = GameState.Paused;
        uiManager?.ShowPauseMenu(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        CurrentState = GameState.InGame;
        uiManager?.ShowPauseMenu(false);
    }
*/
    public void AddKill()
    {
        enemiesKilledThisWave++;
    }

    public void SpendResources(int amount)
    {
        resourceCount -= amount;
        resourceCount = Mathf.Max(0, resourceCount);
    }

    public void EarnMoney(int amount)
    {
        playerMoney += amount;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }
}