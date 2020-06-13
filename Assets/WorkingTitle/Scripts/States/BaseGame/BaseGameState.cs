using PersonalFramework;
using UnityEngine;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;

    private GameObject[,] m_gridArray;
    private GridSpawnerSettings m_gridSpawnerSettings = new GridSpawnerSettings(5,5);

    private Entity[] m_entities = null;


    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()

    {
       m_gridArray = GridSpawner.SpawnGrid(m_gridSpawnerSettings);
        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        m_entities = EntityFactory.CreateEntities(teamSettings);
    }
    
    protected override void FixedUpdateActiveState()
    {
        Physics.Simulate(Time.fixedDeltaTime);
    }
}
