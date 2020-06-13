using Unity.Mathematics;
using UnityEngine;

using static UnityEngine.Random;

public static class EntityFactory
{
    public static Entity[] CreateEntities(TeamSettings teamSettings)
    {
        Entity[] entities = new Entity[teamSettings.m_entitiesPerTeam * teamSettings.m_teamCount];
        SetDebugEntities(entities);
        return entities;
    }

    private static void SetDebugEntities(Entity[] entities)
    {
        Transform characterParent = new GameObject("Players").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Player");
        EquipableData[] equipables = Resources.LoadAll<EquipableData>("EquipableData/");
        CharacterData[] characters = Resources.LoadAll<CharacterData>("CharacterData/");

        for(int i = 0; i < entities.Length; i++)
        {
            Vector3 randStart = new Vector3(Range(0, 3f), 1.0f, Range(0f, 3f));
            GameObject go = Object.Instantiate(characterPrefab, randStart, quaternion.identity, characterParent);
            CharacterData charData = characters[Range(0, characters.Length)];
            EquipableData equipData = equipables[Range(0, equipables.Length)];
            int2 startTile = new int2(0, 0);

            entities[i] = new Entity(go, charData, equipData, startTile);
        }
    }
}
