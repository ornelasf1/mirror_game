using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public  TextMeshProUGUI scoreText;

    public void ShowGameOver() {
        gameObject.SetActive(true);
        scoreText.text = $"Score {GameManager.Instance.gameData.EnemyKills}";
        GameManager.Instance.gameData.IsGameActive = false;
    }

    public void RestartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
