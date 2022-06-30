using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagName
{
    public static class TestSave
    {
        public const string testCharSave = "TestCharSave";
    }
    
    public static class Inventory
    {
        public const string m_GunSaveData_Mode1 = "m_GunSaveData_Mode1";
        public const string m_GunSaveData_Mode2 = "m_GunSaveData_Mode2";
        public const string m_GunSaveData_Mode3 = "m_GunSaveData_Mode3";

        public const string m_CurrentGun_Mode1 = "m_CurrentGun_Mode1";
        public const string m_CurrentGun_Mode2 = "m_CurrentGun_Mode2";
        public const string m_CurrentGun_Mode3 = "m_CurrentGun_Mode3";
    }
}

[System.Serializable]
public class CharacterSaveData
{
    public int m_ID;
    public string m_Name;
    public int m_Level;
    public List<CharSkill> m_CharSkills;

    // public CharacterSaveData(int mID, string mName, int mLevel, List<CharSkill> mCharSkills)
    // {
    //     m_ID = mID;
    //     m_Name = mName;
    //     m_Level = mLevel;
    //     m_CharSkills = mCharSkills;
    // }
}

[System.Serializable]
public class CharSkill
{
    public int m_ID;
    public string m_SkillName;
    public int m_Level;
    public string m_Ability;

    // public CharSkill(int mID, string mSkillName, int mLevel, string mAbility)
    // {
    //     m_ID = mID;
    //     m_SkillName = mSkillName;
    //     m_Level = mLevel;
    //     m_Ability = mAbility;
    // }
}