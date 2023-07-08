//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace GameMain
{
    [Serializable]
    public abstract class AccessoryObjectData : EntityData
    {
        [SerializeField]
        protected int m_OwnerId = 0;

        public AccessoryObjectData(int entityId, int typeId, int ownerId)
            : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
        }

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }
    }
}