using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GameMain
{
	[CreateAssetMenu,NodeWidth(400)]
    public class BehaviorNode : Node
    {
        [Input] public float a;
        [Output] public float b;
		[SerializeField, Output(dynamicPortList = true)]
		private List<BehaviorTag> behaviors = new List<BehaviorTag>();
		[SerializeField]
		private CatData charData;

		protected override void Init()
		{
			base.Init();

		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}
	}

}