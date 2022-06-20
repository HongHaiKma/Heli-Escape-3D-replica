public class ChaseState3 : IState<Enemy3>
{
    private static ChaseState3 m_Instance;
    private ChaseState3()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static ChaseState3 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new ChaseState3();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy3 _charState)
    {
        _charState.OnChaseEnter();
    }

    public void Execute(Enemy3 _charState)
    {
        _charState.OnChaseExecute();
    }

    public void Exit(Enemy3 _charState)
    {
        _charState.OnChaseExit();
    }
}