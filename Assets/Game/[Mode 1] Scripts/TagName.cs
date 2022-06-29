using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagName
{
    public static class TestSave
    {
        public const string test = "TestCharSave";
    }
}

public class CharacterSave
{
    public string m_Name;
    public int m_Level;
    public List<CharSkill> m_CharSkills;
}

public class CharSkill
{
    public string m_SkillName;
    public int m_Level;
    public string m_Ability;
}