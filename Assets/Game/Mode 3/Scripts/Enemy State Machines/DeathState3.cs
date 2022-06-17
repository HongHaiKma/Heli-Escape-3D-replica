public class DeathState3 : IState<Enemy3>
{
    private static DeathState3 m_Instance;
    private DeathState3()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static DeathState3 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new DeathState3();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy3 _charState)
    {
        _charState.OnDeathEnter();
    }

    public void Execute(Enemy3 _charState)
    {
        _charState.OnDeathExecute();
    }

    public void Exit(Enemy3 _charState)
    {
        _charState.OnDeathExit();
    }
}