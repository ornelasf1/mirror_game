using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject gridCell;
    public int gridSize = 8;
    private float cellSize = 1.3f; // approx size of cell square asset

    // Start is called before the first frame update
    void Start()
    {
        initializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void initializeGrid()
    {

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject newCell = Instantiate(gridCell, new Vector3(transform.position.x + (i * cellSize), transform.position.y + (j * cellSize), transform.position.z), Quaternion.identity, transform.parent);
                newCell.GetComponent<SpriteRenderer>().name = $"Cell-{i}-{j}";
            }
        }
    }
}
