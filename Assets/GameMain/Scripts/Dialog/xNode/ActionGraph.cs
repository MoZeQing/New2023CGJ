using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class ActionGraph : NodeGraph {
	[SerializeField]
	public CharSO charSO;
	[SerializeField]
	public Trigger trigger;
}