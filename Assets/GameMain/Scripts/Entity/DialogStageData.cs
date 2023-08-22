using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class DialogStageData : EntityData
    {
        public DialogueGraph DialogueGraph
        {
            get;
            set;
        }

        public DialogStageData(int entityId, int typeId,DialogueGraph dialogueGraph)
            : base(entityId, typeId)
        {
            DialogueGraph= dialogueGraph;
        }
    }
}