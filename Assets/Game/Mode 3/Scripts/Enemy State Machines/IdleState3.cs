public class IdleState3 : IState<Enemy3>
{
    private static IdleState3 m_Instance;
    private IdleState3()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static IdleState3 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new IdleState3();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy3 _charState)
    {
        _charState.OnIdleEnter();
    }

    public void Execute(Enemy3 _charState)
    {
        _charState.OnIdleExecute();
    }

    public void Exit(Enemy3 _charState)
    {
        _charState.OnIdleExit();
    }
}