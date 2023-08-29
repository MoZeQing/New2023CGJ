using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LittleCharData : EntityData
    {
        public TeachingForm TeachingForm
        {
            get;
            set;
        }

        public CharSO CharSO
        {
            get;
            set;
        }

        public LittleCharData(int entityId, int tpyeId,CharSO charSO)
            :base(entityId, tpyeId)
        { 

        }
    }
}
