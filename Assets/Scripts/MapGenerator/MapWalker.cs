


using UnityEngine;

public enum Direction {
    UP, DOWN, LEFT, RIGHT
}

public class MapWalker {
    private int mapWidth;
    private int mapHeight;
    public int[][] Grid {get; set;}

    private const int maxNumberOfUnitsToWalkPerWall = 10;
    private int UnitsWalkedSoFar = 0;
    private float chanceToChangeDirection = 0.5f;
    private float ChanceToBeginWalking = 0.05f;
    private readonly Direction? LastDirection = null;

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
        float value = Random.Range(0f, 1f);
        if (value > ChanceToBeginWalking) {
            return;
        }
        Debug.Log($"Start at {x},{y}");
        BeginWalking(x, y);
        UnitsWalkedSoFar = 0;
    }

    private bool BeginWalking(int x, int y) {
        if (Grid[x][y] == 1) {
            Debug.Log($"Step back from {x},{y}");
            return true;
        }
        if (UnitsWalkedSoFar > maxNumberOfUnitsToWalkPerWall) {
            Debug.Log($"Done at {x},{y}");
            return false;
        }
        if (IsInCornerBounds(x, y)) {
            return false;
        }

        if (!IsInBounds(x, y)) {
            return false;
        }
        
        Direction direction;
        if (IsInBottomBounds(x, y)) {
            direction = Direction.UP;
        } else if (IsInLeftBounds(x, y)) {
            direction = Direction.RIGHT;
        } else if (IsInRightBounds(x, y)) {
            direction = Direction.LEFT;
        } else if (IsInTopBounds(x, y)) {
            direction = Direction.DOWN;
        } else {
            direction = ChooseDirection();
        }
        return Walk(x, y, direction);
    }

    public bool Walk(int x, int y, Direction direction) {
        UnitsWalkedSoFar++;
        Grid[x][y] = 1;
        Debug.Log($"Set unit in {x},{y}. Go {direction}");
        bool isNextStepTaken = true;
        if (isNextStepTaken && direction == Direction.UP) {
            isNextStepTaken = BeginWalking(x, y+1);
        }
        if (isNextStepTaken && direction == Direction.RIGHT) {
            isNextStepTaken = BeginWalking(x+1, y);
        }
        if (isNextStepTaken && direction == Direction.DOWN) {
            isNextStepTaken = BeginWalking(x, y-1);
        }
        if (isNextStepTaken && direction == Direction.LEFT) {
            isNextStepTaken = BeginWalking(x-1, y);
        }
        return isNextStepTaken;  
    }

    private Direction ChooseDirection() {
        float value = Random.Range(0f, 1f);
        Direction? newDirection = LastDirection;
        while(newDirection == LastDirection) {
            if (value >= 0.75f) {
                newDirection = Direction.UP;
            } else if(value >= 0.5f) {
                newDirection = Direction.LEFT;
            } else if(value >= 0.25f) {
                newDirection = Direction.RIGHT;
            } else {
                newDirection = Direction.DOWN;
            }
        }
        return (Direction) newDirection;
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
}