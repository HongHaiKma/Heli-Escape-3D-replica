using UnityEngine;

public class P_RunState : IState<Hostage>
{
    private static P_RunState m_Instance;
    private P_RunState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_RunState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_RunState();
            }

            return m_Instance;
        }
    }

    public void Enter(Hostage _charState)
    {
        _charState.OnRunEnter();
    }

    public void Execute(Hostage _charState)
    {
        _charState.OnRunExecute();
    }

    public void Exit(Hostage _charState)
    {
        _charState.OnRunExit();
    }
}