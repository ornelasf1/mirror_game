using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData = new()
    {
        EnemyKills = 0,
        IsGameActive = true,
    };
    public static GameManager Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
        
    }

    public void IncreaseKill() {
        gameData.EnemyKills++;
    }

    public bool IsGameActive() {
        return gameData.IsGameActive;
    }
}
