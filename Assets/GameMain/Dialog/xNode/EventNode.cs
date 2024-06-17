using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class EventNode : Node {
	[Input] public float a;
	[Output] public float b;

	public EventData eventData;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}