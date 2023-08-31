using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    public Week week;
    [SerializeField]
    public Trigger trigger;
    [SerializeField]
    public LevelData levelData;
}
