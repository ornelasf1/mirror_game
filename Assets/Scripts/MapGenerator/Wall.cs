
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wall {
    private Vector2 StartPos;
    private Vector2 EndPos;
    public int Units { get; }

    public Wall(Vector2 _startPos, Vector2 _endPos) {
        StartPos = _startPos;
        EndPos = _endPos;
        Units = Math.Abs((int)_endPos.x - (int)_startPos.x) + Math.Abs((int)_endPos.y - (int)_startPos.y);
    }
}

public class CompositeWall {
    private const int maxNumberOfUnits = 10;
    public int RemainingNumberOfUnits { get; private set; }

    private List<Wall> walls;

    public void CreateWall(Wall wall) {
        walls.Add(wall);
        RemainingNumberOfUnits -= wall.Units;
    }
}