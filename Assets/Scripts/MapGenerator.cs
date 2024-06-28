using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallUnit;
    private readonly int GridSize = 60;

    private float cellWidth;
    private float cellHeight;
    private MapWalker mapWalker;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform mapZone = gameObject.GetComponent<RectTransform>();
        cellWidth = mapZone.rect.width / GridSize;
        cellHeight = mapZone.rect.height / GridSize;
        Vector3 pos = wallUnit.transform.localPosition;
        pos.x = 10;
        wallUnit.transform.localScale = new Vector3(1, 5, 1);
        wallUnit.transform.localPosition = pos;
        mapWalker = new MapWalker(GridSize, GridSize);
        initializeGrid();
        wallUnit.transform.localScale = new Vector3(cellWidth, cellHeight, 1);
        for (int i = 0; i < mapWalker.Grid.Length; i++)
        {
            for (int j = 0; j < mapWalker.Grid[i].Length; j++)
            {
                if (mapWalker.Grid[i][j] == 1) {
                    Debug.Log($"Spawn at x:{(i+1) * cellWidth} y:{(j+1) * cellHeight}");
                    Vector3 wallPos = new Vector3((i+1) * cellWidth, (j+1) * cellHeight, 0);
                    Instantiate(wallUnit, transform.TransformPoint(wallPos), Quaternion.identity, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void initializeGrid()
    {

        for (int y = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                mapWalker.AttemptWalking(x, y);
                // GameObject newCell = Instantiate(gridCell, new Vector3(
                //     transform.position.x + (i * cellWidth), 
                //     transform.position.y + (j * cellHeight), 
                //     transform.position.z), Quaternion.identity, transform);
                // GameObject newCell = Instantiate(gridCell, new Vector3(
                //     j * cellWidth, 
                //     i * cellHeight, 
                //     transform.position.z), Quaternion.identity, transform);
                // newCell.GetComponent<SpriteRenderer>().name = $"Cell-{i}-{j}";
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
}
