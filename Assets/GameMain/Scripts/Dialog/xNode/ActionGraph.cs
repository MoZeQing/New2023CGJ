using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class ActionGraph : NodeGraph {
	[SerializeField]
	public Trigger trigger;
}