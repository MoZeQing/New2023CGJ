using Dialog;
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
        [SerializeField] private const string m_CsvDialogPath="Dialog/CSV";
        //[SerializeField] private const string m_ExcelDialogPath;
        [SerializeField] private const string m_XNodeDialogPath = "Dialog/XNode";

        private List<StorySO> m_Stories=new List<StorySO>();
        private List<StorySO> m_LoadedStories=new List<StorySO>();
        
        private Dictionary<string,DialogData> m_MapsDialogs= new Dictionary<string,DialogData>();

        private Action mAction;

        public int GetLoadedStoryCount
        {
            get { return m_LoadedStories.Count; }
        }
        public int GetLoadedDialogCount
        {
            get { return m_MapsDialogs.Count; }
        }
        public int GetAllStoryCount
        {
            get { return m_Stories.Count; }
        }

        public List<string> LoadedStories
        { 
            get
            {
                List<string> newStory = new List<string>();
                foreach (StorySO storySO in m_LoadedStories)
                {
                    newStory.Add(storySO.name);
                }
                return newStory;
            }
        }

        public void LoadCsvDialogData()
        {
            TextAsset[] dialogTexts = Resources.LoadAll<TextAsset>(m_CsvDialogPath);
            foreach (TextAsset dialogText in dialogTexts)
            {
                IDialogSerializeHelper helper = new CSVSerializeHelper();
                DialogData dialogData = helper.Serialize(dialogText.text);
                if (!m_MapsDialogs.ContainsKey(dialogData.DialogName))
                    m_MapsDialogs.Add(dialogData.DialogName, dialogData);
                else
                    Debug.LogError($"错误，存在多个具有相同名称的剧情文件，请确认{dialogData.DialogName}");
            }
        }

        public void LoadExcelDialogData()
        {

        }

        public void LoadXNodeDialogData()
        {
            DialogueGraph[] dialogueGraphs = Resources.LoadAll<DialogueGraph>(m_XNodeDialogPath);
            foreach (DialogueGraph dialogueGraph in dialogueGraphs)
            {
                IDialogSerializeHelper helper = new XNodeSerializeHelper();
                DialogData dialogData = helper.Serialize(dialogueGraph);
                if (dialogData == null)
                {
                    Debug.LogError($"错误，{dialogueGraph.name}存在无效数据");
                    continue;
                }
                if (!m_MapsDialogs.ContainsKey(dialogData.DialogName))
                    m_MapsDialogs.Add(dialogData.DialogName, dialogData);
                else
                    Debug.LogError($"错误，存在多个具有相同名称的剧情文件，请确认{dialogData.DialogName.ToString()}");
            }
        }

        public bool InDialog
        {
            get;
            set;
        }

        private void OnEnable()
        {
            LoadCsvDialogData();
            LoadXNodeDialogData();

            foreach (StorySO storySO in Resources.LoadAll<StorySO>("StoryData"))
            {
                if (storySO.unLoad)
                    continue;
                m_Stories.Add(storySO);
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
            foreach (StorySO story in m_LoadedStories)
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
            foreach (StorySO story in m_LoadedStories)
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
                        m_LoadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        public bool StoryUpdate()
        {
            mAction=null;
            foreach (StorySO story in m_LoadedStories)
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
                        m_LoadedStories.Remove(story);
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
            if (m_MapsDialogs.ContainsKey(tag))
            {
                DialogData dialogData = m_MapsDialogs[tag];
                if (GameEntry.UI.HasUIForm(UIFormId.DialogForm))
                {
                    DialogForm df = GameEntry.UI.GetUIForm(UIFormId.DialogForm).GetComponent<DialogForm>();
                    df.SetData(dialogData);
                }
                else
                {
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, dialogData);
                }
                InDialog = true;
                GameEntry.Event.FireNow(this, DialogEventArgs.Create(InDialog, dialogData.DialogName));
                return true;
            }
            return false;
        }

        private void SaveGame(object sender, GameEventArgs e)
        {
            SaveGameEventArgs args = (SaveGameEventArgs)e;
            List<string> newStory = new List<string>();
            foreach (StorySO storySO in m_LoadedStories)
            {
                newStory.Add(storySO.name);
            }
            args.SaveLoadData.storyData= newStory;
        }

        public void LoadGame(List<string> storyData)
        {
            m_LoadedStories.Clear();
            foreach (StorySO storySO in m_Stories)
            {
                if (storyData.Contains(storySO.name))
                { 
                    m_LoadedStories.Add(storySO);
                }
            }
        }

        public void LoadGame()
        {
            m_LoadedStories = new List<StorySO>(m_Stories);
        }
    }
}
