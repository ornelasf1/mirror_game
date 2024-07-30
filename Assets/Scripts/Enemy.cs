using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Gradient gradient;
    SpriteRenderer spriteRenderer;
    public GameObject prefabPlusScoreAnimationText;
    private readonly int maxHealth = 100;
    private float currentHealth;
    private readonly float healthDamageMultiplier = 100f;
    private float bombTimer = 1f;
    private int bombDamage = 20;
    private HealthBar userHealthBar;
    private Transform enemyHealthBar;
    private readonly float disappearHealthBarInSeconds = 1f;
    private float disappearHealthBarElapsed = 0f;
    private int gainPointsPerSecond = 10;
    private int pointsForKilling = 5;
    private GameObject scoreUI;

    void Start() {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        userHealthBar = GameObject.FindGameObjectWithTag("UserHealth").GetComponent<HealthBar>();
        GameObject enemyHealthBarGO = Helpers.FindGameObjectInChildWithTag(transform.gameObject, "EnemyHealth");
        scoreUI = GameObject.FindGameObjectWithTag("ScoreUI");
        if (enemyHealthBarGO != null) {
            enemyHealthBar = enemyHealthBarGO.GetComponent<Transform>();
            enemyHealthBar.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (!GameStateManager.Instance.IsGameActive()) return;
        CountDownEnemyBombTimer();
        CheckIfHealthBarDepleted();
        
        if (disappearHealthBarElapsed <= 0f && enemyHealthBar.transform.parent.gameObject.activeSelf) {
            enemyHealthBar.transform.parent.gameObject.SetActive(false);
        } else if (disappearHealthBarElapsed > 0f) {
            disappearHealthBarElapsed -= Time.deltaTime / disappearHealthBarInSeconds;
        }

        // Run once every second
        // elapsedTime += Time.deltaTime;
        // if (elapsedTime >= 1f) {
        //     elapsedTime %= 1f;
        // }

        // Count down seconds
        // timer += Time.deltaTime;
        // int seconds = (int)(timer % 60);
        // Debug.Log(seconds);
    }

    private void CountDownEnemyBombTimer() {
        if (bombTimer <= 0) {
            userHealthBar.DealDamage(bombDamage);
            Destroy(gameObject);
        } else {
            bombTimer -= Time.deltaTime / GameStateManager.Instance.LevelData.SecondsTilFoeDetonates; // reaches 0f in 10 seconds, Time.deltaTime * 0.1f
            spriteRenderer.color = gradient.Evaluate(bombTimer);
        }
    }

    private void CheckIfHealthBarDepleted() {
        if (currentHealth <= 0) {
            Destroy(gameObject);
            GameStateManager.Instance.IncreaseKill();
            GameStateManager.Instance.LevelData.FoeDied();
            GivePlayerKillPoints();
        }
    }

    public void DealDamage(int amount) {
        if (currentHealth > 0) {
            currentHealth -= amount * healthDamageMultiplier * Time.deltaTime;
            GameStateManager.Instance.IncreaseScore(Time.deltaTime * gainPointsPerSecond);
            if (enemyHealthBar) {
                enemyHealthBar.transform.parent.gameObject.SetActive(true);
                enemyHealthBar.localScale = new Vector3(currentHealth / maxHealth, enemyHealthBar.localScale.y, enemyHealthBar.localScale.z);
            }
            disappearHealthBarElapsed = disappearHealthBarInSeconds;
        }
    }

    private void GivePlayerKillPoints() {
        GameStateManager.Instance.IncreaseScore(pointsForKilling);
        Instantiate(prefabPlusScoreAnimationText, scoreUI.transform);
    }
}
