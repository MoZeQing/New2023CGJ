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
        [SerializeField] private DialogBox dialogBox= null;
        [SerializeField] private BaseStage stage = null;
        [SerializeField] private List<StorySO> stories;
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
                    continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.CloseUIGroup("Default");
                    dialogBox.gameObject.SetActive(true);
                    dialogBox.SetDialog(story.dialogueGraph);
                    stage.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
                    //dialogBox.SetComplete();
                    InDialog= true;
                    if (story.isRemove)
                        stories.Remove(story);
                    return true;
                }
            }
            return false;
        }
    }
}
