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

        private void Start()
        {
            stories= new List<StorySO>(Resources.LoadAll<StorySO>("StoryData"));
        }
        public void StoryUpdate()
        {
            foreach (StorySO story in stories)
            {
                if (GameEntry.Utils.Location != story.outingSceneState)
                    return;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                }
            }
        }
    }
}
