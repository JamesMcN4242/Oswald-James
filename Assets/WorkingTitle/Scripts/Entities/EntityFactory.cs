using Unity.Mathematics;
using UnityEngine;

using static UnityEngine.Random;

public static class EntityFactory
{
    public static EntityContainer CreateEntities(TeamSettings teamSettings)
    {
        EntityContainer entityContainer = new EntityContainer(teamSettings.m_entitiesPerTeam * teamSettings.m_teamCount);
        SetDebugEntities(entityContainer);
        return entityContainer;
    }

    private static void SetDebugEntities(EntityContainer entityContainer)
    {
        Transform characterParent = new GameObject("Players").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Player");
        EquipableData[] equipables = Resources.LoadAll<EquipableData>("EquipableData/");
        CharacterData[] characters = Resources.LoadAll<CharacterData>("CharacterData/");

        for(int i = 0; i < entityContainer.Count; i++)
        {
            Vector3 randStart = new Vector3(Range(-5.5f, 5.5f), 1.0f, Range(-8f, 8f));
            entityContainer.m_gameObjects[i] = Object.Instantiate(characterPrefab, randStart, quaternion.identity, characterParent);
            entityContainer.m_gridPositions[i] = new int2(0, 0);
            entityContainer.m_characterData[i] = characters[Range(0, characters.Length)];
            entityContainer.m_equipableData[i] = equipables[Range(0, equipables.Length)];
        }
    }
}
