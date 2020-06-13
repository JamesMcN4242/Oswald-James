using Unity.Mathematics;
using UnityEngine;

public class Entity
{
    private GameObject m_gameObject;
    private CharacterData m_characterData;
    private EquipableData m_equipableData;
    private int2 m_currentTile;

    public Entity(GameObject go, CharacterData charData, EquipableData equipableData, int2 startingTile)
    {
        m_gameObject = go;
        m_characterData = charData;
        m_equipableData = equipableData;
        m_currentTile = startingTile;
    }
}
