using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class CharacterData : EntityData
    {

        public ActionGraph ActionGraph
        {
            get;
            set;
        }
        public CharacterData(int entityId, int typeId,ActionGraph actionGraph)
            : base(entityId, typeId)
        {
            this.ActionGraph= actionGraph;
        }
    }
}

