using UnityEngine;
using System.Collections.Generic;


public class ScenerySpawner 
{
    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public static Tile[,] SpawnScenery(Tile[,] gridArray, GridSpawnerSettings gridSpawnerSettings)
    {
        List<Unity.Mathematics.int2> generationCoords = new List<Unity.Mathematics.int2>();
        for (int i = 0; i < gridSpawnerSettings.m_x; ++i)
        {
            for (int j = 0; j < gridSpawnerSettings.m_y; ++j)
            {
                if (i == 0 || (i == gridSpawnerSettings.m_x - 1) || j == 0 || j == (gridSpawnerSettings.m_y - 1))
                {
                    gridArray[i,j].m_isPassable = false;
                    gridArray[i,j].m_tileType = Tile.TileType.WALL;

                    if (Random.value > 0.9f)
                    {
                        generationCoords.Add(new Unity.Mathematics.int2(i,j));
                    }
                }
            }
        }
        foreach (Unity.Mathematics.int2 currentCoords in generationCoords)
        {
            GenerateWall(gridArray, currentCoords.x, currentCoords.y);
        }

        return gridArray;
    }

    private static Tile[,] GenerateWall(Tile[,] gridArray, int x, int y)
    {
        Direction direction = Direction.UP;
        float changeDirection = 0.00f;
        float doorChance = 0.00f;
        bool isComplete = false;

        if (x == 0)
        {
            direction = Direction.RIGHT;
        }
        if (x == gridArray.GetUpperBound(0))
        {
            direction = Direction.LEFT;
        }
        if (y == 0)
        {
            direction = Direction.UP;
        }
        if (y == gridArray.GetUpperBound(1))
        {
            direction = Direction.DOWN;
        }

        while (!isComplete)
        {
            switch (direction)
            {
                case Direction.UP:
                    y += 1;
                    break;
                case Direction.DOWN:
                    y -= 1;
                    break;
                case Direction.LEFT:
                    x -= 1;
                    break;
                case Direction.RIGHT:
                    x += 1;
                    break;
                default:
                    isComplete = true;
                    break;
            }

            if (gridArray[x,y].m_tileType == Tile.TileType.WALL)
            {
                isComplete = true;
            }
            else
            {
                gridArray[x,y].m_tileType = Tile.TileType.WALL;
                gridArray[x,y].m_isPassable = false;

                if (changeDirection > Random.value)
                {
                    if ((changeDirection % 0.02f) == 0)
                    {
                        if (direction == Direction.UP)
                        {
                            direction = Direction.LEFT;
                        }
                        else if (direction == Direction.DOWN)
                        {
                            direction = Direction.RIGHT;
                        }
                        else if (direction == Direction.LEFT)
                        {
                            direction = Direction.DOWN;
                        }
                        else if (direction == Direction.RIGHT)
                        {
                            direction = Direction.UP;
                        }
                    }
                    else
                    {
                        if (direction == Direction.UP)
                        {
                            direction = Direction.RIGHT;
                        }
                        else if (direction == Direction.DOWN)
                        {
                            direction = Direction.LEFT;
                        }
                        else if (direction == Direction.LEFT)
                        {
                            direction = Direction.UP;
                        }
                        else if (direction == Direction.RIGHT)
                        {
                            direction = Direction.DOWN;
                        }
                    }
                    changeDirection = 0.00f;
                }
                else
                {
                    changeDirection += 0.05f;
                }

                if (doorChance > Random.value)
                {
                    gridArray[x,y].m_tileType = Tile.TileType.DOOR;
                    gridArray[x,y].m_isPassable = true;
                    doorChance = 0.00f;
                }
                else
                {
                    doorChance += 0.05f;
                }
            }
        }

        return gridArray;

    }
}
