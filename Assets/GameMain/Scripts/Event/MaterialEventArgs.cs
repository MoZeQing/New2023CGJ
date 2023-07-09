using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class MaterialEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(MaterialEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public NodeTag NodeTag
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public static MaterialEventArgs Create(NodeTag nodeTag,int value)
        {
            MaterialEventArgs args = ReferencePool.Acquire<MaterialEventArgs>();
            args.NodeTag = nodeTag;
            args.Value = value;
            return args;
        }

        public override void Clear()
        {

        }
    }
}