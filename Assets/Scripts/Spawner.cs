using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    private float elapsedTime = 0f;

    private RectTransform spawnZone;
    [SerializeField] private TextMeshProUGUI scoreTMP;

    void Start() {
        spawnZone = GetComponent<RectTransform>();
        UpdateScore(0);
        GameStateManager.Instance.onNewScore += UpdateScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.Instance.IsGameActive()) {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > GameStateManager.Instance.LevelData.IntervalForFoeSpawnInMs / 1000f) {
            elapsedTime = 0f;
            for (int spawnCount = 0; spawnCount < Random.Range(1, GameStateManager.Instance.LevelData.MaxNumberOfFoesToSpawnAtOnce + 1); spawnCount++)
            {
                for (int retries = 0; retries < 10; retries++)
                {
                    float yPos = Random.Range(spawnZone.rect.yMin, spawnZone.rect.yMax);
                    float xPos = Random.Range(spawnZone.rect.xMin, spawnZone.rect.xMax);
                    Vector3 newWorldPoint = spawnZone.TransformPoint(new Vector3(xPos, yPos, -1f));
                    newWorldPoint.z = -1f;
                    if (!Physics2D.OverlapCircle(newWorldPoint, 2.5f, LayerMask.GetMask("Foe","Obstacle"))) {
                        Instantiate(enemy, newWorldPoint, Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }

    private void UpdateScore(int newScore) {
        scoreTMP.text = $"Score {newScore}";
    }
}
