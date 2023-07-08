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

        private static void InitCustomComponents()
        {
            Utils = UnityGameFramework.Runtime.GameEntry.GetComponent<UtilsComponent>();
        }
    }
}
