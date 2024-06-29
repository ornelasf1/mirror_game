using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallUnit;
    private int GridSize = 60;

    private float cellWidth;
    private float cellHeight;
    private RectTransform mapZone;
    private MapWalker mapWalker;
    private MapWalkerArgs currentArgs;

    // Start is called before the first frame update
    void Start()
    {
        mapZone = gameObject.GetComponent<RectTransform>();
        cellWidth = mapZone.rect.width / GridSize;
        cellHeight = mapZone.rect.height / GridSize;
        mapWalker = new MapWalker(GridSize, GridSize, new MapWalkerArgs {
            MaxNumberOfUnitsToWalkPerWall = 10,
        });
        currentArgs = mapWalker.MapWalkerArgs;
        InitializeGrid();
        InstantiateWalls();
        GameStateManager.Instance.LevelData.onNewLevel += RedrawMap;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitializeGrid()
    {

        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                mapWalker.AttemptWalking(x, y);
            }
        }
        string result = "";
        for (int i = 0; i < mapWalker.Grid.Length; i++)
        {
            string line = " ";
            for (int j = 0; j < mapWalker.Grid[i].Length; j++)
            {
                line += $"{mapWalker.Grid[i][j]}" + (j == mapWalker.Grid[i].Length - 1 ? "" : ",");
            }
            result += line + "|";
        }
        Debug.Log(result);
    }

    private void InstantiateWalls() {
        for (int i = 0; i < mapWalker.Grid.Length; i++)
        {
            for (int j = 0; j < mapWalker.Grid[i].Length; j++)
            {
                if (mapWalker.Grid[i][j] == 1) {
                    Debug.Log($"Spawn at x:{(i+1) * cellWidth} y:{(j+1) * cellHeight}");
                    Vector3 wallPos = new Vector3((i+1) * cellWidth, (j+1) * cellHeight, 0);
                    Collider2D collider = Physics2D.OverlapPoint(transform.TransformPoint(wallPos), 1 << LayerMask.NameToLayer("SpawnFreeZone"));
                    if (!collider) {
                        Instantiate(wallUnit, transform.TransformPoint(wallPos), Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    private void CleanUpWalls() {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
                Destroy(child.gameObject);
            }
        }
    }

    private void RedrawMap(int newLevel) {
        CleanUpWalls();
        GridSize += 2;
        cellWidth = mapZone.rect.width / GridSize;
        cellHeight = mapZone.rect.height / GridSize;
        mapWalker = new MapWalker(GridSize, GridSize, new MapWalkerArgs {
            MaxNumberOfUnitsToWalkPerWall = currentArgs.MaxNumberOfUnitsToWalkPerWall + 1,
            WallProximityRadius = Math.Max(5, currentArgs.WallProximityRadius - 1),
            MaxSizeOfWall = currentArgs.MaxSizeOfWall + 2,
            MinSizeOfWall = currentArgs.MinSizeOfWall + 2,
            ChanceToBeginWalking = currentArgs.ChanceToBeginWalking + 0.02f,
        });
        currentArgs = mapWalker.MapWalkerArgs;
        InitializeGrid();
        InstantiateWalls();
    }
}
