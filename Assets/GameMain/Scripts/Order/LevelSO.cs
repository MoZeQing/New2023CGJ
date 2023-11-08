using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSO : ScriptableObject
{
    [SerializeField]
    public bool isRandom;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public LevelData levelData;
}
