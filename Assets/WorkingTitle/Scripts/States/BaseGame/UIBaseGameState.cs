using PersonalFramework;
using TMPro;

public class UIBaseGameState : UIStateBase
{
    private const float k_tooLongTime = 180.0f;
    private TextMeshProUGUI m_timerText = null;

    void Start()
    {
        m_timerText = gameObject.GetComponentFromChild<TextMeshProUGUI>("TimerText");
    }

    public void SetTimerText(float time)
    {
        if (time < k_tooLongTime)
        {
            int minutes = (int)(time / 60f);
            float seconds = time % 60f;
            m_timerText.text = $"{minutes} minutes and {seconds.ToString("N1")} seconds";
        }
        else
        {
            m_timerText.text = "Too long";
        }
    }
}
