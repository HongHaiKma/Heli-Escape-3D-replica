using UnityEngine;

public class ChaseState : IState<Enemy>
{
    private static ChaseState m_Instance;
    private ChaseState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static ChaseState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new ChaseState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnChaseEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnChaseExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnChaseExit();
    }
}