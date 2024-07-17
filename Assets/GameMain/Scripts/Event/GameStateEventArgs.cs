using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GameStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameStateEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GameState GameState
        {
            get;
            set;
        }

        public static GameStateEventArgs Create(GameState gameState)
        {
            GameStateEventArgs args = ReferencePool.Acquire<GameStateEventArgs>();
            args.GameState = gameState;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

public enum GameState
{
    None,
    Morning,//早上轮次
    Work,//工作轮次
    ForeSpecial,//特殊客人前轮次
    Special,//特殊客人轮次
    AfterSpecial,//特殊客人后轮次
    Afternoon,//下午轮次
    Night,//傍晚轮次
    Midnight,//晚上轮次
    Sleep,
    Guide,
    Menu,
    Weekend
}
