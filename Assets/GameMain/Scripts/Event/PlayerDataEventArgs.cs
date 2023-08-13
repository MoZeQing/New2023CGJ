using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class PlayerDataEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerDataEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public PlayerData PlayerData
        {
            get;
            set;
        }

        public static PlayerDataEventArgs Create(PlayerData playerData)
        {
            PlayerDataEventArgs args = ReferencePool.Acquire<PlayerDataEventArgs>();
            args.PlayerData = playerData;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
