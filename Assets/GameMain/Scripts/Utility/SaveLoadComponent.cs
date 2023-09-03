using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameMain
{
    public class SaveLoadComponent : GameFrameworkComponent
    {
        //规定，其中0为自动存档、1~4为玩家手动存档的位置
        private SaveLoadData[] mSaveLoadData = new SaveLoadData[5];

        private void Start()
        {
            LoadGame();
        }
        public void SaveGame(int index)
        {
            SaveLoadData saveLoadData = new SaveLoadData();
            GameEntry.Event.FireNow(this, SaveGameEventArgs.Create(saveLoadData));
            DateTime dateTime= DateTime.Now;
            saveLoadData.dataTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            saveLoadData.day = GameEntry.Utils.Day;
            saveLoadData.playerData = GameEntry.Utils.PlayerData;
            saveLoadData.charData= GameEntry.Utils.CharData;
            saveLoadData.flags= GameEntry.Utils.Flags;
            saveLoadData.workDatas = GameEntry.Utils.WorkDatas;
            saveLoadData.storyData = GameEntry.Dialog.LoadedStories;
            mSaveLoadData[index]= saveLoadData;
            SaveGame();
        }

        private void SaveGame()
        {
            GameData gameData = new GameData(mSaveLoadData);
            FileStream fs = File.Create(Application.persistentDataPath + "/save.sav");
            BinaryFormatter bf=new BinaryFormatter();
            bf.Serialize(fs, gameData);
            fs.Close();
        }

        public SaveLoadData LoadGame(int index)
        {
            LoadGame();
            if (mSaveLoadData[index] == null)
            {
                Debug.LogWarning("错误，不存在该存档文件");
                return null;
            }
            SaveLoadData saveLoadData = mSaveLoadData[index];
            GameEntry.Event.FireNow(this, LoadGameEventArgs.Create(saveLoadData));
            return saveLoadData;
        }
        private void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/save.sav"))
            {
                FileStream fs = File.OpenRead(Application.persistentDataPath + "/save.sav");
                BinaryFormatter bf = new BinaryFormatter();
                GameData gameData = (GameData)bf.Deserialize(fs);
                mSaveLoadData = gameData.saveLoadData;
                fs.Close();
            }
        }
    }
    /// <summary>
    /// 需要保存的数据
    /// </summary>
    [System.Serializable]
    public class SaveLoadData
    {
        public string dataTime;
        public int day;
        public CharData charData;
        public PlayerData playerData;
        public List<string> storyData = new List<string>();
        public List<WorkData> workDatas= new List<WorkData>();
        public List<string> flags=  new List<string>();
    }
    [System.Serializable]
    public class GameData
    {
        public SaveLoadData[] saveLoadData = new SaveLoadData[5];

        public GameData() { }
        public GameData(SaveLoadData[] saveLoadData) 
        { 
            this.saveLoadData = saveLoadData;
        }
    }
}
