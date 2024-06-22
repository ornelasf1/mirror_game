using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ScreenBounds
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    private ScreenBounds bounds;

    private float elapsedTime = 0f;

    private float spawnPerSecond = 2f;

    void Start() {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0.05f, 0.05f));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(0.95f, 0.8f)); // a little off the top to avoid the topmost canvas
        bounds = new ScreenBounds {
            left = bottomLeft.x,
            right = topRight.x,
            bottom = bottomLeft.y,
            top = topRight.y
        };
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

            float yPos = Random.Range(bounds.bottom, bounds.top);
            float xPos = Random.Range(bounds.left, bounds.right);
            Instantiate(enemy, new Vector3(xPos, yPos, 0f), Quaternion.identity);
        }
    }
}
