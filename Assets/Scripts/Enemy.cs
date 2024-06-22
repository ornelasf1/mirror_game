using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Gradient gradient;
    SpriteRenderer spriteRenderer;
    private readonly int maxHealth = 100;
    private float currentHealth;
    private readonly float healthDamageMultiplier = 100f;
    private float bombTimer = 1f;
    public int bombDamage = 5;
    private HealthBar userHealthBar;
    private Transform enemyHealthBar;
    private readonly float secondsTilExplosion = 10f;
    private readonly float disappearHealthBarInSeconds = 1f;
    private float disappearHealthBarElapsed = 0f;

    void Start() {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        userHealthBar = GameObject.FindGameObjectWithTag("UserHealth").GetComponent<HealthBar>();
        GameObject enemyHealthBarGO = Helpers.FindGameObjectInChildWithTag(transform.gameObject, "EnemyHealth");
        if (enemyHealthBarGO != null) {
            enemyHealthBar = enemyHealthBarGO.GetComponent<Transform>();
            enemyHealthBar.transform.parent.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (!GameManager.Instance.IsGameActive()) return;
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
            bombTimer -= Time.deltaTime / secondsTilExplosion; // reaches 0f in 10 seconds, Time.deltaTime * 0.1f
            spriteRenderer.color = gradient.Evaluate(bombTimer);
        }
    }

    private void CheckIfHealthBarDepleted() {
        if (currentHealth <= 0) {
            Destroy(gameObject);
            GameManager.Instance.IncreaseKill();
        }
    }

    public void DealDamage(int amount) {
        if (currentHealth > 0) {
            currentHealth -= amount * healthDamageMultiplier * Time.deltaTime;
            if (enemyHealthBar) {
                enemyHealthBar.transform.parent.gameObject.SetActive(true);
                enemyHealthBar.localScale = new Vector3(currentHealth / maxHealth, enemyHealthBar.localScale.y, enemyHealthBar.localScale.z);
            }
            disappearHealthBarElapsed = disappearHealthBarInSeconds;
        }
    }
}
