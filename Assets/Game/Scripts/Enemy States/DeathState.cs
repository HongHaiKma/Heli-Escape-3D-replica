using UnityEngine;

public class DeathState : IState<Enemy>
{
    private static DeathState m_Instance;
    private DeathState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static DeathState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new DeathState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnDeathEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnDeathExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnDeathExit();
    }
}