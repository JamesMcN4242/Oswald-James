using UnityEngine;

public static class GridSpawner
{
    public static Tile[,] SpawnGrid(GridSpawnerSettings gridSpawnerSettings)
    {
        Tile m_tile = Resources.Load<Tile>("Level/Tile");
        Tile[,] m_gridArray = new Tile[gridSpawnerSettings.m_x,gridSpawnerSettings.m_y];
        Transform m_gridparent = new GameObject("Tiles").transform;
        
        for (int i = 0; i < gridSpawnerSettings.m_x; ++i)
        {
            Vector3 spawnPos = gridSpawnerSettings.m_origin;
            spawnPos.x += i * gridSpawnerSettings.m_xIncrement;
            for (int j = 0; j < gridSpawnerSettings.m_y; ++j)
            {
                m_gridArray[i,j] = Object.Instantiate(m_tile, spawnPos, Quaternion.identity, m_gridparent);
                spawnPos.z += gridSpawnerSettings.m_yIncrement;
            }
            
        }
        return m_gridArray;
    }
}

public struct GridSpawnerSettings
{
    public int m_x;
    public int m_y;
    public Vector3 m_origin;
    public float m_xIncrement;
    public float m_yIncrement;

    public GridSpawnerSettings(int x, int y)
    {
        this.m_x = x;
        this.m_y = y;
        this.m_origin = new Vector3(0.0f, 0.0f, 0.0f);
        this.m_xIncrement = 1.0f;
        this.m_yIncrement = 1.0f;
    }

    public GridSpawnerSettings(int x, int y, Vector3 origin)
    {
        this.m_x = x;
        this.m_y = y;
        this.m_origin = origin;
        this.m_xIncrement = 1.0f;
        this.m_yIncrement = 1.0f;
    }

    public GridSpawnerSettings(int x, int y, Vector3 origin, float xIncrement, float yIncrement)
    {
        this.m_x = x;
        this.m_y = y;
        this.m_origin = origin;
        this.m_xIncrement = xIncrement;
        this.m_yIncrement = yIncrement;
    }
}