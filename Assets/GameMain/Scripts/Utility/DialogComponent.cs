using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogComponent : GameFrameworkComponent
    {      
        private List<StorySO> stories=new List<StorySO>();
        private List<StorySO> loadedStories=new List<StorySO>();

        private Action mAction;

        public List<string> LoadedStories
        { 
            get
            {
                List<string> newStory = new List<string>();
                foreach (StorySO storySO in loadedStories)
                {
                    newStory.Add(storySO.name);
                }
                return newStory;
            }
        }
        public bool InDialog
        {
            get;
            set;
        }

        private void OnEnable()
        {
            stories=new List<StorySO>(Resources.LoadAll<StorySO>("StoryData"));
        }

        public void OnComplete()
        {
            if(mAction!=null)
                mAction();
            mAction=null;
        }

        public bool StoryUpdate(Action action)
        {
            mAction = action;
            return StoryUpdate();
        }

        public bool StoryUpdate()
        {
            foreach (StorySO story in loadedStories)
            {
                if(story.outingSceneState!=OutingSceneState.Main)
                    if (GameEntry.Utils.Location != story.outingSceneState)
                        continue;
                if (GameEntry.Utils.outingBefore != story.outingBefore)
                    if (GameEntry.Utils.Location == OutingSceneState.Home)
                        continue;
                //if (GameEntry.Utils.TimeTag != story.timeTag)
                //    if (GameEntry.Utils.TimeTag != TimeTag.None)
                //        continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    Debug.Log(story.name);
                    Debug.Log(story.dialogueGraph.name);
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                    InDialog = true;
                    GameEntry.Event.FireNow(this, DialogEventArgs.Create(InDialog, story.dialogueGraph.name));
                    foreach (EventData eventData in story.eventDatas)
                    {
                        GameEntry.Utils.RunEvent(eventData);
                    }
                    if (story.isRemove)
                        loadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        public bool PlayStory(string tag)
        {
            foreach (StorySO story in loadedStories)
            {
                if (story.name == tag)
                {
                    if (GameEntry.UI.HasUIForm(UIFormId.DialogForm))
                    {
                        DialogForm df = GameEntry.UI.GetUIForm(UIFormId.DialogForm).GetComponent<DialogForm>();
                        df.SetData(story.dialogueGraph);
                    }
                    else
                    {
                        GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                    }
                    InDialog = true;
                    GameEntry.Event.FireNow(this, DialogEventArgs.Create(InDialog, story.dialogueGraph.name));
                    if (story.isRemove)
                        loadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        private void SaveGame(object sender, GameEventArgs e)
        {
            SaveGameEventArgs args = (SaveGameEventArgs)e;
            List<string> newStory = new List<string>();
            foreach (StorySO storySO in loadedStories)
            {
                newStory.Add(storySO.name);
            }
            args.SaveLoadData.storyData= newStory;
        }

        public void LoadGame(List<string> storyData)
        {
            loadedStories.Clear();
            foreach (StorySO storySO in stories)
            {
                if (storyData.Contains(storySO.name))
                { 
                    loadedStories.Add(storySO);
                }
            }
        }

        public void LoadGame()
        {
            loadedStories = new List<StorySO>(stories);
        }
    }
}
