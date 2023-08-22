using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogComponent : GameFrameworkComponent
    {
        public List<StorySO> stories;
        public bool InDialog
        {
            get;
            set;
        }

        private void Start()
        {
            stories= new List<StorySO>(Resources.LoadAll<StorySO>("StoryData"));
        }

        public bool StoryUpdate()
        {
            foreach (StorySO story in stories)
            {
                if (GameEntry.Utils.Location != story.outingSceneState)
                    return false;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.CloseUIGroup("Default");
                    GameEntry.Entity.ShowDialogStage(new DialogStageData(GameEntry.Entity.GenerateSerialId(), 10010, story.dialogueGraph));
                    InDialog= true;
                    return true;
                }
            }
            return false;
        }
    }
}
