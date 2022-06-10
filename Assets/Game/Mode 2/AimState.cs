using UnityEngine;

public class AimState : IState<Enemy2>
{
    private static AimState m_Instance;
    private AimState()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static AimState Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new AimState();
            }

            return m_Instance;
        }
    }

    public void Enter(Enemy2 _charState)
    {
        _charState.OnAimEnter();
    }

    public void Execute(Enemy2 _charState)
    {
        _charState.OnAimExecute();
    }

    public void Exit(Enemy2 _charState)
    {
        _charState.OnAimExit();
    }
}