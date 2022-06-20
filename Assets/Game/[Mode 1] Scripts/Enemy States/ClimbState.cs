using UnityEngine;

public class ClimbState : IState<Enemy>
{
    private static ClimbState m_Instance;
    private ClimbState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static ClimbState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new ClimbState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnClimbEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnClimbExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnClimbExit();
    }
}