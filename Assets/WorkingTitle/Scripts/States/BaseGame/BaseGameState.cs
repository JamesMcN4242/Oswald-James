using PersonalFramework;
using UnityEngine;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;

    private GameObject[,] m_gridArray;

    private Entity[] m_entities = null;


    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()

    {
       m_gridArray = GridSpawner.SpawnGrid(5,5, new Vector3(0.0f, 0.0f, 0.0f));
        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        m_entities = EntityFactory.CreateEntities(teamSettings);
    }
    
    protected override void FixedUpdateActiveState()
    {
        Physics.Simulate(Time.fixedDeltaTime);
    }
}
