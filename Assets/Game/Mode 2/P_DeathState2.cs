using UnityEngine;

public class P_DeathState2 : IState<Shooter2>
{
    private static P_DeathState2 m_Instance;
    private P_DeathState2()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_DeathState2 Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_DeathState2();
            }

            return m_Instance;
        }
    }

    public void Enter(Shooter2 _charState)
    {
        _charState.OnDeathEnter();
    }

    public void Execute(Shooter2 _charState)
    {
        _charState.OnDeathExecute();
    }

    public void Exit(Shooter2 _charState)
    {
        _charState.OnDeathExit();
    }
}