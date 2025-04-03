using System;
using UnityEngine;
using System.Collections.Generic;
using Alchemy.Serialization;
using Unity.VisualScripting;

[AlchemySerialize]
public partial class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    // === Game State ===
    public enum GameState { MainMenu, InGame, Paused, Shop, GameOver, LevelComplete }
    public GameState CurrentState { get; private set; }
    public int currentWave;

    // === Engine & Trailer Setup ===
    [AlchemySerializeField, NonSerialized]
    public Dictionary<TrailerSO, TurretSO> selectedTrailerPairs;
    public List<TrailerSO> runtimeTrailers;

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
        
        runtimeTrailers.Clear();

        foreach (var trailerPair in selectedTrailerPairs)
        {
            TrailerSO trailer = Instantiate(trailerPair.Key);
            trailer.turret = trailerPair.Value;
            runtimeTrailers.Add(trailer);
        }
        player.trailers = runtimeTrailers;
        CalculatePlayerHP();
    }

    public void CalculatePlayerHP()
    {
        float totalHP = 0;

        foreach (var trailer in runtimeTrailers)
        {
            totalHP += trailer.stats.GetBasicStat(Stat.Health)??0;
        }

        player.gameObject.GetComponent<Stats>().SetBasicStat(Stat.Health, totalHP);

        Debug.Log($"[GameHandler] Total HP: {totalHP}");
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