using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class DialogStageData : EntityData
    {
        public CharSO CharSO
        {
            get;
            set;
        }

        public DialogStageData(int entityId, int typeId)
            : base(entityId, typeId)
        {

        }
    }
}