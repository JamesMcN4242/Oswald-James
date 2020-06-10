using PersonalFramework;

public class LocalGameDirector : LocalDirector
{
    void Start()
    {
        m_stateController.PushState(new BaseGameState());
    }
}
