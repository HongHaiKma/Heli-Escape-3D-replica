using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
// using Newtonsoft.Json;

public class PlayerProfile
{
    private BigNumber m_Gold = new BigNumber(0);
    public string ic_Gold = "0";

    private BigNumber m_Keys = new BigNumber(0);
    public string ic_Keys = "0";

    public int m_Level;
    public int m_Ads;

    public void LoadLocalProfile()
    {
        m_Gold = new BigNumber(ic_Gold);
        m_Keys = new BigNumber(ic_Keys);

        // if (GetCharacterProfile(CharacterType.BATMAN) != null)
        // {
        //     Helper.DebugLog("Batman existed!!!");
        // }
        // int a = 2;
        // a = data1["m_Gold"].To;
    }

    public void CreateNewPlayer()
    {
        // PlayerPrefs.SetInt(ConfigKeys.noAds, 0);
        // PlayerPrefs.SetInt(ConfigKeys.rateUs, 1);

        m_Level = 1;
        m_Ads = 1;
    }

    public void SaveDataToLocal()
    {
        string piJson = this.ObjectToJsonString();
        ProfileManager.Instance.SaveDataText(piJson);
    }

    public string ObjectToJsonString()
    {
        return JsonMapper.ToJson(this);
    }

    public JsonData StringToJsonObject(string _data)
    {
        return JsonMapper.ToObject(_data);
    }

    #region GOLD
    public BigNumber GetGold()
    {
        return m_Gold;
    }

    public string GetGold(bool a = false)
    {
        return (m_Gold + 1).ToString();
    }

    public bool IsEnoughGold(BigNumber _value)
    {
        // _value += 0;
        return (m_Gold >= _value);
    }

    public void AddGold(BigNumber _value)
    {
        m_Gold += _value;
        ic_Gold = m_Gold.ToString();
        // ProfileManager.Instance.SaveData();
        SaveDataToLocal();
        // EventManager.TriggerEvent("UpdateGold");
    }

    public void ConsumeGold(BigNumber _value)
    {
        m_Gold -= _value;
        ic_Gold = m_Gold.ToString();
        // ProfileManager.Instance.SaveData();
        SaveDataToLocal();
        // EventManager.TriggerEvent("UpdateGold");
    }

    public void SetGold(BigNumber _value)
    {
        m_Gold = _value;
        ic_Gold = m_Gold.ToString();
        // ProfileManager.Instance.SaveData();
        SaveDataToLocal();
        // EventManager.TriggerEvent("UpdateGold");
    }

    #endregion

    #region 

    public void AddKeys(BigNumber _value)
    {
        m_Keys += _value;
        ic_Keys = m_Keys.ToString();
        SaveDataToLocal();
    }

    public BigNumber GetKeys()
    {
        return m_Keys;
    }

    #endregion

    public bool CheckAds()
    {
        if (m_Ads == 1)
        {
            return true;
        }

        return false;
    }

    public void SetAds(int _value)
    {
        m_Ads = _value;
        SaveDataToLocal();
    }

    #region LEVEL

    public void PassLevel()
    {
        if (m_Level < 1)
        {
            m_Level++;
        }

        SaveDataToLocal();
    }

    public int GetLevel()
    {
        return m_Level;
    }

    public void SetLevel(int _level)
    {
        m_Level = _level;
        SaveDataToLocal();
    }

    public void SetKeys(BigNumber _keys)
    {
        m_Keys = _keys;
        SaveDataToLocal();
    }

    #endregion
}