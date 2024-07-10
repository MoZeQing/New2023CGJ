using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class StoryEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(StoryEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string StoryName
        {
            get;
            set;
        }

        public static StoryEventArgs Create(string storyName)
        {
            StoryEventArgs args = ReferencePool.Acquire<StoryEventArgs>();
            args.StoryName = storyName;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
