using UnityEngine;

public static class GridSpawner
{
    public static GameObject[,] SpawnGrid(int x, int y, Vector3 origin, float xIncrement = 1.0f, float yIncrement = 1.0f)
    {
        GameObject m_tile = Resources.Load<GameObject>("Level/Tile");
        GameObject[,] m_gridArray = new GameObject[x,y];
        
        for (int i = 0; i < x; ++i)
        {
            Vector3 spawnPos = origin;
            spawnPos.x += i * xIncrement;
            for (int j = 0; j < y; ++j)
            {
                spawnPos.z += origin + (j * yIncrement);
                m_gridArray[i,j] = Object.Instantiate(m_tile, spawnPos, Quaternion.identity);
            }
        }
        return m_gridArray;
    }
}
