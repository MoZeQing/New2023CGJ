using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ValueEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ValueEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public TriggerTag TriggerTag
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public static ValueEventArgs Create(TriggerTag triggerTag, string value)
        {
            ValueEventArgs args = ReferencePool.Acquire<ValueEventArgs>();
            args.TriggerTag = triggerTag;
            args.Value = value;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
