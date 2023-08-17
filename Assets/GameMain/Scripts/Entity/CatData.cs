using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CatData : EntityData
    {
        public CharSO CharSO
        {
            get;
            set;
        }

        public CatData(int entityId, int typeId)
            : base(entityId, typeId)
        {

        }
    }
}
