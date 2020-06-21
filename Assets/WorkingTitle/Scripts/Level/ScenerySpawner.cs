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
        
        for (int i = 0; i < gridSpawnerSettings.m_x; ++i)
        {
            BoundaryWall(gridArray, i, 0, generationCoords);

            BoundaryWall(gridArray, i, (gridSpawnerSettings.m_y - 1), generationCoords);
            
        }
        for (int i = 0; i < gridSpawnerSettings.m_y; ++i)
        {

            BoundaryWall(gridArray, 0, i, generationCoords);

            BoundaryWall(gridArray, (gridSpawnerSettings.m_x - 1), i, generationCoords);

        }
        foreach (Unity.Mathematics.int2 currentCoords in generationCoords)
        {
            GenerateWall(gridArray, currentCoords.x, currentCoords.y);
        }

    }

    private static void BoundaryWall(Tile[,] gridArray, int x, int y, List<Unity.Mathematics.int2> generationCoords)
    {
        if (gridArray[x,y].m_tileType == Tile.TileType.WALL)
        {
            return;
        }
        const float chanceOfNoWall = 0.9f;
        Mesh mesh = gridArray[x,y].GetComponent<MeshFilter>().mesh;
        List<Vector3> vertices = new List<Vector3>();
        mesh.GetVertices(vertices);

        for (int n = 0; n < vertices.Count; ++n)
        {
            vertices[n] = new Vector3(vertices[n].x, vertices[n].y * 4, vertices[n].z);
        }
        mesh.SetVertices(vertices, 0, vertices.Count);

        gridArray[x,y].m_isPassable = false;
        gridArray[x,y].m_tileType = Tile.TileType.WALL;

        if (Random.value > chanceOfNoWall)
        {
            generationCoords.Add(new Unity.Mathematics.int2(x,y));
        }

    }

    private static void GenerateWall(Tile[,] gridArray, int x, int y)
    {
        Direction direction = Direction.UP;
        const float increment = 0.03f;
        const float oddOrEven = 0.02f;
        const float halfwallToggleThreshold = 0.9f;
        bool halfwallToggle = false;
        float changeDirection = 0.00f;
        float doorChance = 0.00f;
        bool isComplete = false;

        if (x == 0)
        {
            direction = Direction.RIGHT;
        }
        else if (x == gridArray.GetUpperBound(0))
        {
            direction = Direction.LEFT;
        }
        else if (y == 0)
        {
            direction = Direction.UP;
        }
        else if (y == gridArray.GetUpperBound(1))
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

            if (gridArray[x,y].m_tileType != Tile.TileType.FLOOR)
            {
                isComplete = true;
            }
            else
            {
                Mesh mesh = gridArray[x,y].GetComponent<MeshFilter>().mesh;
                List<Vector3> vertices = new List<Vector3>();
                mesh.GetVertices(vertices);
                
                if (halfwallToggle)
                { 
                    gridArray[x,y].m_tileType = Tile.TileType.HALFWALL;
                    for (int n = 0; n < vertices.Count; ++n)
                    {
                        vertices[n] = new Vector3(vertices[n].x, vertices[n].y * 2, vertices[n].z);
                    }
                    mesh.SetVertices(vertices, 0, vertices.Count);        
                }
                else 
                {
                    gridArray[x,y].m_tileType = Tile.TileType.WALL;
                    for (int n = 0; n < vertices.Count; ++n)
                    {
                        vertices[n] = new Vector3(vertices[n].x, vertices[n].y * 4, vertices[n].z);
                    }
                    mesh.SetVertices(vertices, 0, vertices.Count);
                }
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

                    for (int n = 0; n < vertices.Count; ++n)
                    {
                        if (halfwallToggle)
                        {
                            vertices[n] = new Vector3(vertices[n].x, vertices[n].y / 2, vertices[n].z);
                        }
                        else
                        {
                            vertices[n] = new Vector3(vertices[n].x, vertices[n].y / 4, vertices[n].z);
                        }
                    }
                    mesh.SetVertices(vertices, 0, vertices.Count);
                }
                else
                {
                    doorChance += increment;
                }

                if (Random.value > halfwallToggleThreshold)
                {
                    halfwallToggle = !halfwallToggle;
                }
            }
        }

    }
}
