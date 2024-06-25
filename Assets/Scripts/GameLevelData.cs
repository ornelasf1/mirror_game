
using System;

public class GameLevelData {
    public int ActiveLevel { get; private set; } = 1;
    public int RemainingFoes { get; private set; }
    public float SecondsTilFoeDetonates { get; private set; } = 10f;
    public float IntervalForFoeSpawnInMs { get; private set; } = 5000f;
    public int MaxNumberOfFoesToSpawnAtOnce { get; private set; } = 1;

    private const int maxAmountOfFoesInALevel = 50;
    private const int minAmountOfFoesToSpawnInALevel = 10;
    private const float minSecondsTilFoeDetonate = 3f;

    public GameLevelData() {
        RemainingFoes = minAmountOfFoesToSpawnInALevel;
    }
    
    public void AdvanceToNextLevel() {
        ActiveLevel++;
        RemainingFoes = CalculateNextFoeAmount(ActiveLevel);
        SecondsTilFoeDetonates = CalculateSecondsTilDetonate(ActiveLevel);
    }

    public void FoeDied() {
        RemainingFoes--;
    }

    private int CalculateNextFoeAmount(int level) {
        return (int) Math.Min(maxAmountOfFoesInALevel, Math.Floor(minAmountOfFoesToSpawnInALevel + (Math.Pow(level, 2f) * 0.1f))); // y = x^2/10 = ax^c
    }

    private float CalculateSecondsTilDetonate(int level) {
        return (float) Math.Max(minSecondsTilFoeDetonate, SecondsTilFoeDetonates - (Math.Pow(level, 1.3f) * 0.1f));
    }
}