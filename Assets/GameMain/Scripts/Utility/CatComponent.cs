using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class CatComponent : GameFrameworkComponent
{
    private int m_Favour = 0;
    private int m_Mood = 0;
    private int m_Hope = 0;
    private int m_Love = 0;
    private int m_Family = 0;

    private ActionGraph m_ActionGraph = null;

    public int Family
    {
        get
        { 
            return m_Family;
        }
        private set { }
    }
    public int Love
    {
        get
        { 
            return m_Love;
        }
        private set { }
    }
    public int Hope
    {
        get
        { 
            return m_Hope;
        }
        private set { }
    }
    public int Favour
    {
        get
        { 
            return m_Favour;
        }
        private set { }
    }

    public int Mood
    {
        get
        {
            return m_Mood;
        }
        private set { }
    }
}
[System.Serializable]
public class CatData
{
    public int favour = 0;
    public int mood = 0;
    public int hope = 0;
    public int love = 0;
    public int family = 0;
}
[System.Serializable]
public class PlayerData
{
    public int energy;
    public int money;
    public int time;
}
public class Behavior
{
    public ActionTag action;
    public CatData catData;
}

public enum ActionTag
{ 
    Click,
    Talk,
    Touch,
    Play,
    Story,
    Sleep
}