using UnityEngine;

public class P_WaitState : IState<Hostage>
{
    private static P_WaitState m_Instance;
    private P_WaitState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_WaitState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_WaitState();
            }

            return m_Instance;
        }
    }

    public void Enter(Hostage _charState)
    {
        _charState.OnWaitEnter();
    }

    public void Execute(Hostage _charState)
    {
        _charState.OnWaitExecute();
    }

    public void Exit(Hostage _charState)
    {
        _charState.OnWaitExit();
    }
}