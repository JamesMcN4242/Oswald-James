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

        int halfEntityLength = entities.Length / 2;
        for (int i = 0; i < entities.Length; i++)
        {
            int2 startTile = new int2(i / halfEntityLength, i % halfEntityLength);
            GameObject go = Object.Instantiate(characterPrefab, new Vector3(startTile.x, 0.0f, startTile.y), quaternion.identity, characterParent);
            go.name = $"Player{i}";
            CharacterData charData = characters[Range(0, characters.Length)];
            EquipableData equipData = equipables[Range(0, equipables.Length)];

            entities[i] = new Entity(go, charData, equipData, startTile);
        }
    }
}
