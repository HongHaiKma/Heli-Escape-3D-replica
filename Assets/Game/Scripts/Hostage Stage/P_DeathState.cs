using UnityEngine;

public class P_DeathState : IState<Hostage>
{
    private static P_DeathState m_Instance;
    private P_DeathState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_DeathState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_DeathState();
            }

            return m_Instance;
        }
    }

    public void Enter(Hostage _charState)
    {
        _charState.OnDeathEnter();
    }

    public void Execute(Hostage _charState)
    {
        _charState.OnDeathExecute();
    }

    public void Exit(Hostage _charState)
    {
        _charState.OnDeathExit();
    }
}