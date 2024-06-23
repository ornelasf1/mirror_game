using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    private float elapsedTime = 0f;

    private float spawnPerSecond = 2f;

    private RectTransform spawnZone;

    void Start() {
        spawnZone = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameActive()) {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > spawnPerSecond) {
            elapsedTime = 0f;

            for (int retries = 0; retries < 3; retries++)
            {
                float yPos = Random.Range(spawnZone.rect.yMin, spawnZone.rect.yMax);
                float xPos = Random.Range(spawnZone.rect.xMin, spawnZone.rect.xMax);
                Vector3 newWorldPoint = spawnZone.TransformPoint(new Vector3(xPos, yPos, -1f));
                newWorldPoint.z = -1f;
                if (!Physics2D.OverlapCircle(newWorldPoint, 2.5f, 1 << LayerMask.NameToLayer("Foe"))) {
                    Instantiate(enemy, newWorldPoint, Quaternion.identity);
                    break;
                }
            }

        }
    }
}
