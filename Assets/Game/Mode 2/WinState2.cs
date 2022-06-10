using UnityEngine;

public class WinState2 : IState<Enemy2>
{
    private static WinState2 m_Instance;
    private WinState2()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static WinState2 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new WinState2();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy2 _charState)
    {
        _charState.OnWinEnter();
    }

    public void Execute(Enemy2 _charState)
    {
        _charState.OnWinExecute();
    }

    public void Exit(Enemy2 _charState)
    {
        _charState.OnWinExit();
    }
}