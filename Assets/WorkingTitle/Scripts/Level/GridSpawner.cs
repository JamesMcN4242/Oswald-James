using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridSpawner
{
    public static void SpawnGrid(int x, int y)
    {
        GameObject m_tile = Resources.Load<GameObject>("Level/Tile");
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < y; ++j)
            {
                Object.Instantiate(m_tile, new Vector3(m_tile.transform.position.x + i, m_tile.transform.position.y, m_tile.transform.position.z + j), Quaternion.identity);
            }
        }
    }
}
