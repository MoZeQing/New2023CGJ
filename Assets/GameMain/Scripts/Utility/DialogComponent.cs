using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogComponent : GameFrameworkComponent
    {      
        private List<StorySO> mStories=new List<StorySO>();
        private List<StorySO> mLoadedStories=new List<StorySO>();
        private Action mAction;

        public List<string> LoadedStories
        { 
            get
            {
                List<string> newStory = new List<string>();
                foreach (StorySO storySO in mLoadedStories)
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
            foreach (StorySO storySO in Resources.LoadAll<StorySO>("StoryData"))
            {
                if (storySO.unLoad)
                    continue;
                mStories.Add(storySO);
            }
        }
        public void SetComplete(Action action)
        {
            mAction = action;
        }

        public void OnComplete()
        {
            if(mAction!=null)
                mAction();

            mAction=null;
        }

        public bool CheckOutStory(OutingSceneState outingSceneState)
        {
            foreach (StorySO story in mLoadedStories)
            {
                if (GameEntry.Utils.Location != story.outingSceneState)
                    continue;
                if (GameEntry.Utils.GameState != story.gameState)
                    if (story.gameState != GameState.None)
                        continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    return true;
                }
            }
            return false;
        }

        public bool StoryUpdate(Action action)
        {
            mAction = action;
            foreach (StorySO story in mLoadedStories)
            {
                if (story.outingSceneState != OutingSceneState.Main)
                    if (GameEntry.Utils.Location != story.outingSceneState)
                        continue;
                if (GameEntry.Utils.GameState != story.gameState)
                    if (story.gameState != GameState.None)
                        continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                    InDialog = true;
                    GameEntry.Event.FireNow(this, DialogEventArgs.Create(InDialog, story.dialogueGraph.name));
                    GameEntry.Event.FireNow(this, StoryEventArgs.Create(story.name));
                    foreach (EventData eventData in story.eventDatas)
                    {
                        GameEntry.Utils.RunEvent(eventData);
                    }
                    if (story.isRemove)
                        mLoadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        public bool StoryUpdate()
        {
            mAction=null;
            foreach (StorySO story in mLoadedStories)
            {
                if(story.outingSceneState!=OutingSceneState.Main)
                    if (GameEntry.Utils.Location != story.outingSceneState)
                        continue;
                if (GameEntry.Utils.GameState != story.gameState)
                    if (story.gameState != GameState.None)
                        continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                    InDialog = true;
                    GameEntry.Event.FireNow(this, DialogEventArgs.Create(InDialog, story.dialogueGraph.name));
                    GameEntry.Event.FireNow(this, StoryEventArgs.Create(story.name));
                    foreach (EventData eventData in story.eventDatas)
                    {
                        GameEntry.Utils.RunEvent(eventData);
                    }
                    if (story.isRemove)
                        mLoadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        public void PlayStory(DialogueGraph dialogueGraph)
        {
            GameEntry.UI.OpenUIForm(UIFormId.DialogForm, dialogueGraph);
        }
        public bool PlayStory(string tag)
        {
            foreach (StorySO story in mLoadedStories)
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
                        mLoadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        private void SaveGame(object sender, GameEventArgs e)
        {
            SaveGameEventArgs args = (SaveGameEventArgs)e;
            List<string> newStory = new List<string>();
            foreach (StorySO storySO in mLoadedStories)
            {
                newStory.Add(storySO.name);
            }
            args.SaveLoadData.storyData= newStory;
        }

        public void LoadGame(List<string> storyData)
        {
            mLoadedStories.Clear();
            foreach (StorySO storySO in mStories)
            {
                if (storyData.Contains(storySO.name))
                { 
                    mLoadedStories.Add(storySO);
                }
            }
        }

        public void LoadGame()
        {
            mLoadedStories = new List<StorySO>(mStories);
        }
    }
}
