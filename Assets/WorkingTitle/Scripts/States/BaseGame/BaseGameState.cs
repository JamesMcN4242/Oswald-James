using PersonalFramework;
using UnityEngine;

public class BaseGameState : FlowStateBase
{
    private UIBaseGameState m_baseUI = null;
    private float m_timeActive = 0.0f;

    protected override bool AquireUIFromScene()
    {
        m_baseUI = GameObject.FindObjectOfType<UIBaseGameState>();
        m_ui = m_baseUI;
        return m_ui != null;
    }

    protected override void UpdateActiveState()
    {
        m_timeActive += Time.deltaTime;
        m_baseUI.SetTimerText(m_timeActive);
    }
}
