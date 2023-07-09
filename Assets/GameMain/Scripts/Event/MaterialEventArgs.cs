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

        public static MaterialEventArgs Create(NodeTag nodeTag)
        {
            MaterialEventArgs args = ReferencePool.Acquire<MaterialEventArgs>();
            args.NodeTag = nodeTag;
            return args;
        }

        public override void Clear()
        {

        }
    }
}