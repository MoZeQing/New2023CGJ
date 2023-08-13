using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class CharDataEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(CharDataEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public CharData CharData
        {
            get;
            set;
        }

        public static CharDataEventArgs Create(CharData charData)
        {
            CharDataEventArgs args = ReferencePool.Acquire<CharDataEventArgs>();
            args.CharData = charData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
