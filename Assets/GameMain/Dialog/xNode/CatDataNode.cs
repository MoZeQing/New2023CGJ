using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace GameMain
{
    [NodeWidth(400)]
    public class CatDataNode : Node
    {
        [Input] public float a;
        [Output] public float b;
        [SerializeField]
        public CatData catData;
        [SerializeField]
        public PlayerData playerData;
        // Use this for initialization
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