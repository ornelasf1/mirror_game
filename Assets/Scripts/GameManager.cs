using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTMP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkIfFoesDefeated();
    }

    private void checkIfFoesDefeated() {
        if (GameStateManager.Instance.LevelData.RemainingFoes <= 0) {
            GameStateManager.Instance.LevelData.AdvanceToNextLevel();
            levelTMP.text = $"Level {GameStateManager.Instance.LevelData.ActiveLevel}";
        }
    }
}
