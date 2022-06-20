using UnityEngine;

public class KillState : IState<Enemy>
{
    private static KillState m_Instance;
    private KillState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static KillState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new KillState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnKillEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnKillExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnKillExit();
    }
}