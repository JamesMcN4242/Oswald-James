using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool m_isPassable = true;

    public enum TileType
    {
        FLOOR,
        WALL,
        DOOR,
        HALFWALL
    }

    public TileType m_tileType;
}
