


using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    UP, DOWN, LEFT, RIGHT
}

public class MapWalker {
    private int mapWidth;
    private int mapHeight;
    public int[][] Grid {get; set;}

    private const int maxNumberOfUnitsToWalkPerWall = 20;
    private int UnitsWalkedSoFar = 0;
    private float chanceToChangeDirection = 0.5f;
    private float ChanceToBeginWalking = 0.05f;
    private Direction? LastDirection = null;
    private List<(int, int)> WallPositions = new();

    public MapWalker(int mapWidth, int mapHeight) {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        Grid = new int[mapWidth][];
        for (int i = 0; i < Grid.Length; i++)
        {
            Grid[i] = new int[mapHeight];
        }
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
            if (UnitsWalkedSoFar < maxNumberOfUnitsToWalkPerWall) {
                throw new InvalidOperationException($"Did not meet quota. {UnitsWalkedSoFar}/{maxNumberOfUnitsToWalkPerWall}");
            }
        }
        catch (InvalidOperationException msg){
            Debug.Log(msg);
            WallPositions.ForEach(pos => {Grid[pos.Item1][pos.Item2] = 0;});
        }
        finally {
            UnitsWalkedSoFar = 0;
            WallPositions.Clear();
            LastDirection = null;
        }
    }

    private bool BeginWalking(int x, int y) {
        if (Grid[x][y] == 1) {
            Debug.Log($"Step back from {x},{y}");
            return false;
        }
        if (UnitsWalkedSoFar > maxNumberOfUnitsToWalkPerWall) {
            Debug.Log($"Done at {x},{y}");
            return true;
        }
        if (IsInCornerBounds(x, y)) {
            throw new InvalidOperationException($"Ended up in corner bounds {x} {y}");
        }

        if (!IsInBounds(x, y)) {
            throw new InvalidOperationException($"Went out of bounds {x} {y}");
        }
        
        Direction direction;
        if (IsInBottomBounds(x, y)) {
            abortEarlyIfUnlucky();
            direction = Direction.UP;
        } else if (IsInLeftBounds(x, y)) {
            abortEarlyIfUnlucky();
            direction = Direction.RIGHT;
        } else if (IsInRightBounds(x, y)) {
            abortEarlyIfUnlucky();
            direction = Direction.LEFT;
        } else if (IsInTopBounds(x, y)) {
            abortEarlyIfUnlucky();
            direction = Direction.DOWN;
        } else {
            direction = ChooseDirection();
        }
        Walk(x, y, direction);
        return true;
    }

    public void Walk(int x, int y, Direction direction) {
        UnitsWalkedSoFar++;
        Grid[x][y] = 1;
        WallPositions.Add((x, y));
        Debug.Log($"Set unit in {x},{y}. Go {direction}");
        bool foundNextSpot = false;
        Direction newDirection = direction;
        var directionsToAttempt = new List<Direction>((Direction[])Enum.GetValues(typeof(Direction)));
        while(!foundNextSpot) {
            if (newDirection == Direction.UP) {
                foundNextSpot = BeginWalking(x, y+1);
            }
            if (newDirection == Direction.RIGHT) {
                foundNextSpot = BeginWalking(x+1, y);
            }
            if (newDirection == Direction.DOWN) {
                foundNextSpot = BeginWalking(x, y-1);
            }
            if (newDirection == Direction.LEFT) {
                foundNextSpot = BeginWalking(x-1, y);
            }
            if (!foundNextSpot && directionsToAttempt.Count != 0) {
                newDirection = directionsToAttempt[UnityEngine.Random.Range(0, directionsToAttempt.Count)];
                directionsToAttempt.Remove(newDirection);
            } else {
                break;
            }
        }
    }

    private bool CheckIfNextStepIsValid(int x, int y) {
        if (Grid[x][y] == 1) {
            return false;
        }
        return true;
    }

    private Direction ChooseDirection() {
        float value = UnityEngine.Random.Range(0f, 1f);
        if (value >= 0.75f) {
            return Direction.UP;
        } else if(value >= 0.5f) {
            return Direction.LEFT;
        } else if(value >= 0.25f) {
            return Direction.RIGHT;
        } else {
            return Direction.DOWN;
        }
    }

    private bool IsInBounds(int x, int y) {
        return x >= 0 && x <= mapWidth && y >= 0 && y <= mapHeight;
    }

    private bool IsInInnerBounds(int x, int y) {
        int gapFromWidthBounds = mapWidth/10;
        int gapFromHeightBounds = mapHeight/10;
        return x >= gapFromWidthBounds && x <= mapWidth - gapFromWidthBounds && y >= gapFromHeightBounds && y <= mapHeight - gapFromHeightBounds;
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

    private void abortEarlyIfUnlucky() {
        if (UnityEngine.Random.Range(0f, 1f) < 0.1f) {
            throw new InvalidOperationException($"Became unlucky from outer bounds");
        } else {
            UnitsWalkedSoFar -= 10;
        }
    }
}