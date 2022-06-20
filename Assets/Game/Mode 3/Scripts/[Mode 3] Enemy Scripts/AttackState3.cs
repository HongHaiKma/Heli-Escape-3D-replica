public class AttackState3 : IState<Enemy3>
{
    private static AttackState3 m_Instance;
    private AttackState3()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static AttackState3 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new AttackState3();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy3 _charState)
    {
        _charState.OnAttackEnter();
    }

    public void Execute(Enemy3 _charState)
    {
        _charState.OnAttackExecute();
    }

    public void Exit(Enemy3 _charState)
    {
        _charState.OnAttackExit();
    }
}