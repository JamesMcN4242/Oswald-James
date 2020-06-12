using Unity.Mathematics;
using UnityEngine;

public class EntityContainer
{
    public CharacterData[] m_characterData;
    public EquipableData[] m_equipableData;
    public GameObject[] m_gameObjects;
    public int2[] m_gridPositions;

    public int Count {get;}

    public EntityContainer(int count)
    {
        Count = count;

        m_characterData = new CharacterData[count];
        m_equipableData = new EquipableData[count];
        m_gameObjects = new GameObject[count];
        m_gridPositions = new int2[count];
    }
}
