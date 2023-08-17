using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StorySO : ScriptableObject
{
    [SerializeField]
    public Trigger trigger;
    [SerializeField]
    public DialogueGraph dialogueGraph;
}
