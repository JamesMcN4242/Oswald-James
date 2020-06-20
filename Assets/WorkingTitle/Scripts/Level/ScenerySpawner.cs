using UnityEngine;
using System.Collections.Generic;


public class ScenerySpawner 
{
    private enum Direction
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,
        COUNT
    }
    public static void SpawnScenery(Tile[,] gridArray, GridSpawnerSettings gridSpawnerSettings)
    {
        List<Unity.Mathematics.int2> generationCoords = new List<Unity.Mathematics.int2>();
        const float chanceOfNoWall = 0.9f;
        for (int i = 0; i < gridSpawnerSettings.m_x; ++i)
        {
            gridArray[i,0].m_isPassable = false;
            gridArray[i,0].m_tileType = Tile.TileType.WALL;

            if (Random.value > chanceOfNoWall)
            {
                generationCoords.Add(new Unity.Mathematics.int2(i,0));
            }

            gridArray[i,(gridSpawnerSettings.m_y - 1)].m_isPassable = false;
            gridArray[i,(gridSpawnerSettings.m_y - 1)].m_tileType = Tile.TileType.WALL;

            if (Random.value > chanceOfNoWall)
            {
                generationCoords.Add(new Unity.Mathematics.int2(i,(gridSpawnerSettings.m_y - 1)));
            }
        }
        for (int i = 0; i < gridSpawnerSettings.m_y; ++i)
        {
            gridArray[0,i].m_isPassable = false;
            gridArray[0,i].m_tileType = Tile.TileType.WALL;

            if (Random.value > chanceOfNoWall)
            {
                generationCoords.Add(new Unity.Mathematics.int2(0,i));
            }

            gridArray[(gridSpawnerSettings.m_x - 1),i].m_isPassable = false;
            gridArray[(gridSpawnerSettings.m_x - 1),i].m_tileType = Tile.TileType.WALL;

            if (Random.value > chanceOfNoWall)
            {
                generationCoords.Add(new Unity.Mathematics.int2((gridSpawnerSettings.m_x - 1),i));
            }
        }
        foreach (Unity.Mathematics.int2 currentCoords in generationCoords)
        {
            GenerateWall(gridArray, currentCoords.x, currentCoords.y);
        }

    }

    private static void GenerateWall(Tile[,] gridArray, int x, int y)
    {
        Direction direction = Direction.UP;
        const float increment = 0.05f;
        const float oddOrEven = 0.02f;
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
                    if ((changeDirection % oddOrEven) == 0)
                    {
                        direction = (Direction)(((int)direction + (int)Direction.COUNT - 1) % (int)Direction.COUNT);
                    }
                    else
                    {
                        direction = (Direction)(((int)direction + 1) % (int)Direction.COUNT);
                    }
                    changeDirection = 0.00f;
                }
                else
                {
                    changeDirection += increment;
                }

                if (doorChance > Random.value)
                {
                    gridArray[x,y].m_tileType = Tile.TileType.DOOR;
                    gridArray[x,y].m_isPassable = true;
                    doorChance = 0.00f;
                }
                else
                {
                    doorChance += increment;
                }
            }
        }

    }
}
