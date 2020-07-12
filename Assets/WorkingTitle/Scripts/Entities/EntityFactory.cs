using Unity.Mathematics;
using UnityEngine;

using static UnityEngine.Random;

public static class EntityFactory
{
    public static Entity[] CreateEntities(TeamSettings teamSettings, float yStartPos)
    {
        Entity[] entities = new Entity[teamSettings.m_entitiesPerTeam * teamSettings.m_teamCount];
        CreateEntities(entities, yStartPos);
        return entities;
    }

    private static void CreateEntities(Entity[] entities, float yStartPos)
    {
        Transform characterParent = new GameObject("Players").transform;
        GameObject characterPrefab = Resources.Load<GameObject>("Prefabs/Player");
        EquipableData[] equipables = Resources.LoadAll<EquipableData>("EquipableData/");
        CharacterData[] characters = Resources.LoadAll<CharacterData>("CharacterData/");
        float yScale = characterPrefab.transform.localScale.y / 2;

        int halfEntityLength = math.max(1, entities.Length / 2);
        for (int i = 0; i < entities.Length; i++)
        {
            int2 startTile = new int2(i / halfEntityLength, i % halfEntityLength);
            GameObject go = Object.Instantiate(characterPrefab, new Vector3(startTile.x, yStartPos + yScale, startTile.y), quaternion.identity, characterParent);
            go.name = $"Player{i}";
            CharacterData charData = characters[Range(0, characters.Length)];
            EquipableData equipData = equipables[Range(0, equipables.Length)];

            entities[i] = new Entity(go, charData, equipData, startTile);

#if UNITY_EDITOR
            if(i < halfEntityLength)
            {
                go.AddComponent<EntityGizmos>().ViewDistance = charData.m_viewDistance;
            }
#endif
        }
    }
}
