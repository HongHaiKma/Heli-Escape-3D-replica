using UnityEngine;

public class P_AimState2 : IState<Shooter2>
{
    private static P_AimState2 m_Instance;
    private P_AimState2()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_AimState2 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_AimState2();
            }

            return m_Instance;
        }
    }

    public void Enter(Shooter2 _charState)
    {
        _charState.OnAimEnter();
    }

    public void Execute(Shooter2 _charState)
    {
        _charState.OnAimExecute();
    }

    public void Exit(Shooter2 _charState)
    {
        _charState.OnAimExit();
    }
}