using UnityEngine;

public class ScenerySpawner 
{
    public static Tile[,] SpawnScenery(Tile[,] gridArray)
    {
        foreach (Tile tile in gridArray)
        {
            if (Random.value > 0.8f)
            {
                tile.m_isPassable = false;
            }
        }

        return gridArray;
    }
}
