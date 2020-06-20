using PersonalFramework;
using UnityEngine;
using Unity.Mathematics;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;

    private Tile[,] m_gridArray;
    private GridSpawnerSettings m_gridSpawnerSettings = new GridSpawnerSettings(50,50);

    private Entity[] m_entities = null;
    private byte m_controlledEntity = byte.MaxValue;


    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()

    {
       m_gridArray = GridSpawner.SpawnGrid(m_gridSpawnerSettings);
       m_gridArray = ScenerySpawner.SpawnScenery(m_gridArray, m_gridSpawnerSettings);
        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        m_entities = EntityFactory.CreateEntities(teamSettings);
        SetControlledEntity(0);
    }

    protected override void UpdateActiveState()
    {
        UpdateDebugMovement();
        UpdateDebugControlledEntity();

        UpdateEntityMovement();
    }

    private void UpdateEntityMovement()
    {
        foreach(Entity entity in m_entities)
        {
            entity.UpdateEntityMovement(Time.deltaTime);
        }
    }

    private void SetControlledEntity(byte newEntity)
    {
        if (newEntity == m_controlledEntity) return;

        if (m_controlledEntity <= m_entities.Length)
        {
            m_entities[m_controlledEntity].SetHighlightStatus(false);
        }

        m_controlledEntity = newEntity;
        m_entities[m_controlledEntity].SetHighlightStatus(true);
    }

    private void UpdateDebugMovement()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            int2 position = m_entities[m_controlledEntity].CurrentTile + new int2(0, 1);
            m_entities[m_controlledEntity].SetNewPosition(position);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int2 position = m_entities[m_controlledEntity].CurrentTile + new int2(0, -1);
            m_entities[m_controlledEntity].SetNewPosition(position);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int2 position = m_entities[m_controlledEntity].CurrentTile + new int2(-1, 0);
            m_entities[m_controlledEntity].SetNewPosition(position);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            int2 position = m_entities[m_controlledEntity].CurrentTile + new int2(1, 0);
            m_entities[m_controlledEntity].SetNewPosition(position);
        }
    }

    private void UpdateDebugControlledEntity()
    {
        for(byte i = 0; i < m_entities.Length && i <= 9; i++)
        {
            if(Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)))
            {
                SetControlledEntity(i);
            }
        }
    }
}
