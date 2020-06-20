using PersonalFramework;
using UnityEngine;
using Unity.Mathematics;

using Random = UnityEngine.Random;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;


    private Tile[,] m_gridArray;
    private InputSystem m_inputSystem = null;
    private CameraSystem m_cameraSystem = null;

    private Entity[] m_entities = null;

    private SelectedGridElement m_selectedGridElement;
    private byte m_controlledEntity = byte.MaxValue;
    private bool m_playerTurn = true;

    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()
    {
        m_inputSystem = new InputSystem();
        m_cameraSystem = new CameraSystem();

        GridSpawnerSettings gridSpawnerSettings = new GridSpawnerSettings(50, 50);
        m_gridArray = GridSpawner.SpawnGrid(gridSpawnerSettings);
        m_gridArray = ScenerySpawner.SpawnScenery(m_gridArray, gridSpawnerSettings);

        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        float yPlayerStart = m_gridArray[0, 0].GetComponent<MeshRenderer>().bounds.size.y + gridSpawnerSettings.m_origin.y;
        m_entities = EntityFactory.CreateEntities(teamSettings, yPlayerStart);
        SetControlledEntity(0);
    }

    protected override void UpdateActiveState()
    {
        UpdateTurn();
        m_cameraSystem.UpdateCamera(m_entities[m_controlledEntity].WorldPosition, Time.deltaTime);
    }

    private void UpdateTurn()
    {
        if (m_playerTurn)
        {
            m_inputSystem.UpdateInput();
            UpdateSelectedGrid();
        }
        else if (!m_entities[m_controlledEntity].IsMoving)
        {
            //TODO: Actual AI Logic - currently choosing random square in range - only positive movement
            int tilesMoveable = m_entities[m_controlledEntity].SpeedStat;
            int newX = Random.Range(0, tilesMoveable);
            tilesMoveable -= newX;
            int newY = Random.Range(0, tilesMoveable);
            int2 newPos = m_entities[m_controlledEntity].CurrentTile + new int2(newX, newY);
            m_entities[m_controlledEntity].SetNewPosition(newPos);
        }

        UpdateEntityMovement();
    }

    private void UpdateSelectedGrid()
    {
        if(m_inputSystem.InputToProcess && !m_entities[m_controlledEntity].IsMoving)
        {        
            SelectedGridElement selected = GridElementSelector.UpdateGridSelection(m_gridArray, m_inputSystem.CurrentMousePosition);
            bool overGridElement = selected.CursorOverElement;
            if ((overGridElement && !selected.m_arrayPos.Equals(m_selectedGridElement.m_arrayPos)) || !overGridElement)
            {
                SetSelectedGridColour(Color.white);
            }
            
            m_selectedGridElement = selected;
            if (overGridElement)
            {
                bool entityInRange = m_entities[m_controlledEntity].CanMoveToNewPosition(selected.m_arrayPos);
                if (m_inputSystem.ClickComplete && entityInRange)
                {
                    m_entities[m_controlledEntity].SetNewPosition(selected.m_arrayPos);
                }
                else
                {
                    SetSelectedGridColour(entityInRange ? Color.magenta : Color.red);
                }
            }
        }
    }
    
    private void UpdateEntityMovement()
    {
        Entity movingEntity = m_entities[m_controlledEntity];
        bool isMoving = movingEntity.IsMoving;
        movingEntity.UpdateEntityMovement(Time.deltaTime);

        if(isMoving && !movingEntity.IsMoving)
        {
            //Switch to the next character
            SetControlledEntity((byte)((m_controlledEntity + 1) % m_entities.Length));
            m_playerTurn = m_controlledEntity < m_entities.Length / 2;
            if(!m_playerTurn)
            {
                SetSelectedGridColour(Color.white);
            }
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

    private void SetSelectedGridColour(Color colour)
    {
        if (m_selectedGridElement.CursorOverElement)
        {
            //TODO: Hold all of the grid element needed data in the collection
            m_gridArray[m_selectedGridElement.m_arrayPos.x, m_selectedGridElement.m_arrayPos.y].GetComponent<MeshRenderer>().material.color = colour;
        }
    }
}
