using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool m_isPassable = true;

    public enum TileType
    {
        FLOOR,
        WALL,
        DOOR
    }

    public TileType m_tileType;
}
