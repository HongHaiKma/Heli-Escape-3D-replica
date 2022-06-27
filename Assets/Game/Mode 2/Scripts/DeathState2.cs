using UnityEngine;

public class DeathState2 : IState<Enemy2>
{
    private static DeathState2 m_Instance;
    private DeathState2()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static DeathState2 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new DeathState2();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy2 _charState)
    {
        _charState.OnDeathEnter();
    }

    public void Execute(Enemy2 _charState)
    {
        _charState.OnDeathExecute();
    }

    public void Exit(Enemy2 _charState)
    {
        _charState.OnDeathExit();
    }
}