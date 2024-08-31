using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

[System.Serializable]
public class BackgroundData : BaseData
{
    public BackgroundTag backgroundTag;
    public int parmOne;
    public int parmTwo;
    public string parmThree;
    public Sprite backgroundSpr;
}

public enum BackgroundTag
{ 
    None,
    Fade
}
