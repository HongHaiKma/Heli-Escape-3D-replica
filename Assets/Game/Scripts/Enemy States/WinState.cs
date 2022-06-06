using UnityEngine;

public class WinState : IState<Enemy>
{
    private static WinState m_Instance;
    private WinState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static WinState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new WinState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy _charState)
    {
        _charState.OnWinEnter();
    }

    public void Execute(Enemy _charState)
    {
        _charState.OnWinExecute();
    }

    public void Exit(Enemy _charState)
    {
        _charState.OnWinExit();
    }
}