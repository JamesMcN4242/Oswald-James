using PersonalFramework;
using UnityEngine;
using Unity.Mathematics;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;

    private InputSystem m_inputSystem = null;
    private GameObject[,] m_gridArray = null;
    private Entity[] m_entities = null;
    private byte m_controlledEntity = byte.MaxValue;

    private SelectedGridElement m_selectedGridElement;
    private byte m_controlledEntity = byte.MaxValue;

    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()
    {
        m_inputSystem = new InputSystem();

        GridSpawnerSettings gridSpawnerSettings = new GridSpawnerSettings(50, 50);
        m_gridArray = GridSpawner.SpawnGrid(gridSpawnerSettings);

        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        float yPlayerStart = m_gridArray[0, 0].GetComponent<MeshRenderer>().bounds.size.y + gridSpawnerSettings.m_origin.y;
        m_entities = EntityFactory.CreateEntities(teamSettings, yPlayerStart);
        SetControlledEntity(0);
    }

    protected override void UpdateActiveState()
    {
        m_inputSystem.UpdateInput();
        UpdateSelectedGrid();        
        UpdateEntityMovement();

        UpdateDebugControlledEntity();
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

    private void SetSelectedGridColour(Color colour)
    {
        if (m_selectedGridElement.CursorOverElement)
        {
            //TODO: Hold all of the grid element needed data in the collection
            m_gridArray[m_selectedGridElement.m_arrayPos.x, m_selectedGridElement.m_arrayPos.y].GetComponent<MeshRenderer>().material.color = colour;
        }
    }
}
