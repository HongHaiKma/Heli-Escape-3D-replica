using UnityEngine;

public class P_JumpToHeli : IState<Hostage>
{
    private static P_JumpToHeli m_Instance;
    private P_JumpToHeli()
    {
        if (m_Instance != null)
        {
            return;
        }

        m_Instance = this;
    }
    public static P_JumpToHeli Instance
    {
        get
        {
            if (m_Instance == null)
            {
                new P_JumpToHeli();
            }

            return m_Instance;
        }
    }

    public void Enter(Hostage _charState)
    {
        _charState.OnJumpToHeliEnter();
    }

    public void Execute(Hostage _charState)
    {
        _charState.OnJumpToHeliExecute();
    }

    public void Exit(Hostage _charState)
    {
        _charState.OnJumpToHeliExit();
    }
}