


using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    UP, DOWN, LEFT, RIGHT
}

public class MapWalkerArgs {
    public float ChanceToBeginWalking {get; set;} = 0.1f;
    public int MinSizeOfWall {get; set;} = 6;
    public int MaxSizeOfWall {get; set;} = 20;
    public int WallProximityRadius {get; set;} = 15;
    public int MaxNumberOfUnitsToWalkPerWall {get; set;} = 20;
}

public class MapWalker {
    private int mapWidth;
    private int mapHeight;
    public int[][] Grid {get; set;}

    public MapWalkerArgs MapWalkerArgs {get;}

    private int MaxNumberOfUnitsToWalkPerWall = 20;
    private int UnitsWalkedSoFar = 0;
    private float ChanceToBeginWalking = 0.1f;
    private Direction? LastDirection = null;
    private List<(int, int)> WallPositions = new();
    private int MinSizeOfWall = 6;
    private int MaxSizeOfWall = 20;
    private int RemainingWallUnits = 0;
    private int WallProximityRadius = 15;
    
    public MapWalker(int mapWidth, int mapHeight, MapWalkerArgs args) {
        MapWalkerArgs = args;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        Grid = new int[mapWidth][];
        for (int i = 0; i < Grid.Length; i++)
        {
            Grid[i] = new int[mapHeight];
        }
        WallProximityRadius = args.WallProximityRadius;
        ChanceToBeginWalking = args.ChanceToBeginWalking;
        MinSizeOfWall = args.MinSizeOfWall;
        MaxSizeOfWall = args.MaxSizeOfWall;
        MaxNumberOfUnitsToWalkPerWall = args.MaxNumberOfUnitsToWalkPerWall;
    }

    public void AttemptWalking(int x, int y) {
        float value = UnityEngine.Random.Range(0f, 1f);
        if (value > ChanceToBeginWalking) {
            return;
        }
        Debug.Log($"Start at {x},{y}");
        try
        {
            BeginWalking(x, y);
            if (UnitsWalkedSoFar < MaxNumberOfUnitsToWalkPerWall) {
                throw new InvalidOperationException($"Did not meet quota. {UnitsWalkedSoFar}/{MaxNumberOfUnitsToWalkPerWall}");
            }
            WallPositions.ForEach(pos => {Grid[pos.Item1][pos.Item2] = 1;});
        }
        catch (InvalidOperationException msg){
            Debug.Log(msg);
            WallPositions.ForEach(pos => {Grid[pos.Item1][pos.Item2] = 0;});
        }
        finally {
            UnitsWalkedSoFar = 0;
            RemainingWallUnits = 0;
            WallPositions.Clear();
            LastDirection = null;
        }
    }

    private bool BeginWalking(int x, int y) {
        if (!IsInBounds(x, y)) {
            throw new InvalidOperationException($"Went out of bounds {x} {y}");
        }

        if (!CheckGridProximity(x, y)) {
            Debug.Log($"Step back from {x},{y}");
            return false;
        }
        if (UnitsWalkedSoFar > MaxNumberOfUnitsToWalkPerWall) {
            Debug.Log($"Done at {x},{y}");
            return true;
        }
        if (IsInCornerBounds(x, y)) {
            throw new InvalidOperationException($"Ended up in corner bounds {x} {y}");
        }
        
        Direction direction = GetDirection(x, y);
        LastDirection = direction;
        
        Walk(x, y, direction);
        return true;
    }

    public void Walk(int x, int y, Direction direction) {
        UnitsWalkedSoFar++;
        Grid[x][y] = 2;
        WallPositions.Add((x, y));
        Debug.Log($"Set unit in {x},{y}. Go {direction}");
        if (direction == Direction.UP) {
            BeginWalking(x, y+1);
        }
        if (direction == Direction.RIGHT) {
            BeginWalking(x+1, y);
        }
        if (direction == Direction.DOWN) {
            BeginWalking(x, y-1);
        }
        if (direction == Direction.LEFT) {
            BeginWalking(x-1, y);
        }
    }

    private Direction GetDirection(int x, int y) {
        if (RemainingWallUnits == 0) {
            RemainingWallUnits = UnityEngine.Random.Range(MinSizeOfWall, MaxSizeOfWall);
            if (IsInBottomBounds(x, y)) {
                return Direction.UP;
            } else if (IsInLeftBounds(x, y)) {
                return Direction.RIGHT;
            } else if (IsInRightBounds(x, y)) {
                return Direction.LEFT;
            } else if (IsInTopBounds(x, y)) {
                return Direction.DOWN;
            } else {
                return GetRandomDirection();
            }
        } else {
            RemainingWallUnits--;
            return LastDirection == null ? GetRandomDirection() : (Direction)LastDirection;
        }
    }

     private Direction GetRandomDirection() {
        Direction? newDireciton = LastDirection;
        while (newDireciton == LastDirection) {
            float value = UnityEngine.Random.Range(0f, 1f);

            if (value >= 0.75f) {
                newDireciton = Direction.UP;
            } else if(value >= 0.5f) {
                newDireciton = Direction.LEFT;
            } else if(value >= 0.25f) {
                newDireciton = Direction.RIGHT;
            } else {
                newDireciton = Direction.DOWN;
            }
        }
        return (Direction)newDireciton;
    }
    private bool CheckGridProximity(int x, int y) {
        for (int i = -WallProximityRadius; i <= WallProximityRadius; i++)
        {
            for (int j = -WallProximityRadius; j <= WallProximityRadius; j++)
            {
                if (!IsInBounds(i+x, j+y)) {
                    continue;
                }
                if (Grid[i+x][j+y] == 1) {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsInBounds(int x, int y) {
        return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
    }

    private bool IsInLeftBounds(int x, int y) {
        int gapFromWidthBounds = mapWidth/10;
        return x < gapFromWidthBounds;
    }

    private bool IsInRightBounds(int x, int y) {
        int gapFromWidthBounds = mapWidth/10;
        return x > mapWidth - gapFromWidthBounds;
    }

    private bool IsInTopBounds(int x, int y) {
        int gapFromHeightBounds = mapHeight/10;
        return y > mapHeight - gapFromHeightBounds;
    }

    private bool IsInBottomBounds(int x, int y) {
        int gapFromHeightBounds = mapHeight/10;
        return y < gapFromHeightBounds;
    }

    private bool IsInCornerBounds(int x, int y) {
        int gapFromWidthBounds = mapWidth/10;
        int gapFromHeightBounds = mapHeight/10;
        return (x <= gapFromWidthBounds && y <= gapFromHeightBounds) || // bottom left
                (x >= mapWidth - gapFromWidthBounds && y <= gapFromHeightBounds) || // bottom right
                (x >= mapWidth - gapFromWidthBounds && y >= mapHeight - gapFromHeightBounds) || // top right
                (x <= gapFromWidthBounds && y >= mapHeight - gapFromHeightBounds); // top left
    }
}