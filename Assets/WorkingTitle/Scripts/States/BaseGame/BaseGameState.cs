using PersonalFramework;
using UnityEngine;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;
    private EntityContainer m_entityContainer = null;

    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void StartPresentingState()
    {
        TeamSettings teamSettings = Resources.Load<TeamSettings>("Settings/TeamSettings");
        m_entityContainer = EntityFactory.CreateEntities(teamSettings);
    }
    
    protected override void FixedUpdateActiveState()
    {
        Physics.Simulate(Time.fixedDeltaTime);
    }
}
