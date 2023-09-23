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
                if (GameEntry.Utils.Check(story.trigger))
                {
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
                Debug.Log(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(story.name.ToString())));
                Debug.Log(Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(tag.ToString())));
                Debug.Log(story.name.ToString()==tag.ToString());
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
