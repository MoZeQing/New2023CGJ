using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogComponent : GameFrameworkComponent
    {
        public List<TriggerData> triggerDatas;

        public void StoryUpdate()
        {
            foreach (TriggerData triggerData in triggerDatas)
            {
                if (GameEntry.Utils.Check(triggerData))
                {
                    foreach (EventData eventData in triggerData.events)
                    { 
                        
                    }
                }
            }
        }
    }
}
