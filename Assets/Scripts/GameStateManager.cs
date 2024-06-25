using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameStateManager : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData LevelData { get; set; }
    public static GameStateManager Instance { get; private set; }
    public delegate void OnNewScore(int newScore);
    public OnNewScore onNewScore;

    void Awake() {
        Instance = this;
        gameData = new()
        {
            EnemyKills = 0,
            Score = 0,
            IsGameActive = true,
        };
        LevelData = new GameLevelData();
    }

    public void IncreaseKill() {
        gameData.EnemyKills++;
    }

    public bool IsGameActive() {
        return gameData.IsGameActive;
    }

    public void IncreaseScore(float increaseBy) {
        gameData.Score += increaseBy;
        onNewScore?.Invoke((int)gameData.Score);
    }
}