public class FallState3 : IState<Enemy3>
{
    private static FallState3 m_Instance;
    private FallState3()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static FallState3 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new FallState3();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy3 _charState)
    {
        _charState.OnFallEnter();
    }

    public void Execute(Enemy3 _charState)
    {
        _charState.OnFallExecute();
    }

    public void Exit(Enemy3 _charState)
    {
        _charState.OnFallExit();
    }
}