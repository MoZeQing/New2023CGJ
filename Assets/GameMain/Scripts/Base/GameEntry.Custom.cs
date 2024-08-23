//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Diagnostics;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static UtilsComponent Utils
        {
            get;
            private set;
        }

        public static DialogComponent Dialog
        {
            get;
            private set;
        }

        public static CatComponent Cat
        {
            get;
            private set;
        }

        public static PlayerComponent Player
        {
            get;
            private set;
        }

        public static SaveLoadComponent SaveLoad
        {
            get;
            private set;    
        }

        public static BuffComponent Buff
        {
            get;
            private set;
        }

        public static LevelComponent Level
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            Utils = UnityGameFramework.Runtime.GameEntry.GetComponent<UtilsComponent>();
            Dialog = UnityGameFramework.Runtime.GameEntry.GetComponent<DialogComponent>();
            Cat = UnityGameFramework.Runtime.GameEntry.GetComponent<CatComponent>();
            SaveLoad = UnityGameFramework.Runtime.GameEntry.GetComponent<SaveLoadComponent>();
            Player = UnityGameFramework.Runtime.GameEntry.GetComponent<PlayerComponent>();
            Buff= UnityGameFramework.Runtime.GameEntry.GetComponent<BuffComponent>();
            Level = UnityGameFramework.Runtime.GameEntry.GetComponent<LevelComponent>();
        }
    }
}
