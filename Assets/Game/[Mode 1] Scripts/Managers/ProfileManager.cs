using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-93)]
public class ProfileManager : MonoBehaviour
{
    private static ProfileManager m_Instance;
    public static ProfileManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public static PlayerProfile MyProfile
    {
        get
        {
            return m_Instance.m_LocalProfile;
        }
    }
    private PlayerProfile m_LocalProfile;

    public BigNumber m_Gold;
    public BigNumber m_Gold2 = new BigNumber(0);

    private void Awake()
    {
        if (m_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            m_Instance = this;
            InitProfile();
            DontDestroyOnLoad(gameObject);
        }

        // MyProfile.AddGold(5f);
    }

    private void Update()
    {
        if (Helper.GetKeyDown(KeyCode.A))
        {
            PassLevel();
        }
        
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     SetSelectedCharacter(2);
        //     Helper.DebugLog("Selected Character: " + GetSelectedCharacter());

        //     // CharacterDataConfig config = GameData.Instance.GetCharacterDataConfig(2);
        //     // Helper.DebugLog("Name " + config.m_Name);
        //     // Helper.DebugLog("Price " + config.m_Price);
        // }

        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     Helper.DebugLog("Selected Character: " + GetSelectedCharacter());
        // }

        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     GUIManager.Instance.LoadPlayScene();
        // }

        // if (Helper.GetKeyDown(KeyCode.A))
        // {
        //     // CharacterData data = GetCharacterProfileData(CharacterType.Frances);
        //     // // for (int i = 0; i < MyProfile.m_CharacterData.Count; i++)
        //     // // {

        //     // //     Helper.DebugLog("ID: " + MyProfile.m_CharacterData[i].m_ID);
        //     // //     Helper.DebugLog("Level: " + MyProfile.m_CharacterData[i].m_Level);
        //     // // }
        //     // Helper.DebugLog("ID: " + data.m_ID);
        //     // Helper.DebugLog("Level: " + data.m_Level);
        //     Helper.DebugLog("Selected Char: " + GetSelectedChar());
        // }
    }

    private void OnEnable()
    {
        StartListenToEvent();
    }

    private void OnDisable()
    {
        StopListenToEvent();
    }

    private void OnDestroy()
    {
        StopListenToEvent();
    }

    public void StartListenToEvent()
    {
        // EventManager.AddListener(GameEvent.END_GAME, PassLevel);
        // EventManagerWithParam<int>.AddListener(GameEvent.EQUIP_CHAR, EquipChar);
    }

    public void StopListenToEvent()
    {
        // EventManager.RemoveListener(GameEvent.END_GAME, PassLevel);
        // EventManagerWithParam<int>.RemoveListener(GameEvent.EQUIP_CHAR, EquipChar);
    }

    public void InitProfile()
    {
        CreateOrLoadLocalProfile();
    }

    private void CreateOrLoadLocalProfile()
    {
        Debug.Log("Create Or Load Data");
        LoadDataFromPref();
    }

    private void LoadDataFromPref()
    {
        Debug.Log("Load Data");
        string dataText = PlayerPrefs.GetString("SuperFetch", "");
        //Debug.Log("Data " + dataText);
        if (string.IsNullOrEmpty(dataText))
        {
            // Dont have -> create new player and save;
            CreateNewPlayer();
        }
        else
        {
            // Have -> Load data
            LoadDataToPlayerProfile(dataText);
        }
    }

    private void CreateNewPlayer()
    {
        m_LocalProfile = new PlayerProfile();
        m_LocalProfile.CreateNewPlayer();
        SaveData();
    }

    private void LoadDataToPlayerProfile(string data)
    {
        m_LocalProfile = JsonMapper.ToObject<PlayerProfile>(data);
        m_LocalProfile.LoadLocalProfile();
        m_Gold = m_LocalProfile.GetGold();
    }

    public void SaveData()
    {
        m_LocalProfile.SaveDataToLocal();
    }

    public void SaveDataText(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            PlayerPrefs.SetString("SuperFetch", data);
        }
    }

    #region Level

    public static int GetLevel()
    {
        return MyProfile.GetLevel();
    }
    
    public static void SetLevel(int _level)
    {
        MyProfile.SetLevel(_level);
    }

    public static void PassLevel()
    {
        MyProfile.PassLevel();
    }

    #endregion
    
    public void TestDisplayGold()
    {
        // string a = 
        // Helper.DebugLog("Profile Gold: " + MyProfile.GetGold());
        // Helper.DebugLog("Profile Level: " + MyProfile.m_Level);
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
    public void OnApplicationQuit()
    {
        SaveData();
    }
}
