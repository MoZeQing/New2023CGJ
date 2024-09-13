using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class CatDataEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(CatDataEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public CatData CatData
        {
            get;
            set;
        }

        public static CatDataEventArgs Create(CatData charData)
        {
            CatDataEventArgs args = ReferencePool.Acquire<CatDataEventArgs>();
            args.CatData = charData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
