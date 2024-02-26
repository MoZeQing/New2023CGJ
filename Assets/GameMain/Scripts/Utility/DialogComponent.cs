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
        public List<LevelSO> levelSOs = new List<LevelSO>();
        public List<LevelSO> loadedLevelSOs = new List<LevelSO>();

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

        public List<string> LoadedLevels
        {
            get
            {
                List<string> newLevel = new List<string>();
                foreach (LevelSO levelSO in loadedLevelSOs)
                {
                    newLevel.Add(levelSO.name);
                }
                return newLevel;
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
            levelSOs = new List<LevelSO>(Resources.LoadAll<LevelSO>("LevelData"));
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
            foreach (StorySO story in loadedStories)
            {
                if (story.outingSceneState != OutingSceneState.Main)
                    if (GameEntry.Utils.Location != story.outingSceneState)
                        continue;
                if (GameEntry.Utils.outingBefore != story.outingBefore)
                    if (GameEntry.Utils.Location == OutingSceneState.Home)
                        continue;
                if (GameEntry.Utils.GameState != story.gameState)
                    if (story.gameState != GameState.None)
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

        public bool StoryUpdate()
        {
            mAction=null;
            foreach (StorySO story in loadedStories)
            {
                if(story.outingSceneState!=OutingSceneState.Main)
                    if (GameEntry.Utils.Location != story.outingSceneState)
                        continue;
                if (GameEntry.Utils.outingBefore != story.outingBefore)
                    if (GameEntry.Utils.Location == OutingSceneState.Home)
                        continue;
                if (GameEntry.Utils.GameState != story.gameState)
                    if (story.gameState != GameState.None)
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

        public void LoadGame(List<string> storyData,List<string> levelData)
        {
            loadedStories.Clear();
            foreach (StorySO storySO in stories)
            {
                if (storyData.Contains(storySO.name))
                { 
                    loadedStories.Add(storySO);
                }
            }
            loadedLevelSOs.Clear();
            foreach (LevelSO levelSO in levelSOs)
            {
                if (levelData.Contains(levelSO.name))
                {
                    loadedLevelSOs.Add(levelSO);
                }
            }
        }

        public void LoadGame()
        {
            loadedStories = new List<StorySO>(stories);
            loadedLevelSOs = new List<LevelSO>(levelSOs);
        }
    }
}
