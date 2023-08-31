using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomEventSO : ScriptableObject
{
    [SerializeField]
    public RandomEvent randomEvent;
}
[System.Serializable]
public class RandomEvent
{
    public Trigger trigger;
    public string text;
    public GameMain.CharData charData;
    public PlayerData playerData;
}
