using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public  TextMeshProUGUI scoreText;

    public void ShowGameOver() {
        gameObject.SetActive(true);
        scoreText.text = $"Score {(int)GameStateManager.Instance.gameData.Score}";
        GameStateManager.Instance.gameData.IsGameActive = false;
    }

    public void RestartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
