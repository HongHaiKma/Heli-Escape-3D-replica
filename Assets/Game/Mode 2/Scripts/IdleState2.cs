using UnityEngine;

public class IdleState2 : IState<Enemy2>
{
    private static IdleState2 m_Instance;
    private IdleState2()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static IdleState2 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new IdleState2();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy2 _charState)
    {
        _charState.OnIdleEnter();
    }

    public void Execute(Enemy2 _charState)
    {
        _charState.OnIdleExecute();
    }

    public void Exit(Enemy2 _charState)
    {
        _charState.OnIdleExit();
    }
}