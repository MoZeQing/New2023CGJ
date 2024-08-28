using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogConfigScriptableObject")]
public class DialogConfigScriptableObject : ScriptableObject
{
    [SerializeField] public DialogueGraph TestDialogueGraph { get; set; }
}
