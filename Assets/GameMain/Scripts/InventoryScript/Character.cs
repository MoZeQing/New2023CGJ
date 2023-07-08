using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Inventory/New Character")]
public class Character : ScriptableObject
{
    public int ATK;
    public int life;
    public int DEF;
    public int Infection;
    
    [TextArea]
    public string Info;
    public bool Action;
}
