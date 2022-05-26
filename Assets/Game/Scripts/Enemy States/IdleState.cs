using UnityEngine;

public class IdleState : IState<Enemy>
{
    private static IdleState m_Instance;
    private IdleState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static IdleState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new IdleState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnIdleEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnIdleExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnIdleExit();
    }
}