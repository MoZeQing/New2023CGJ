using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices.ComTypes;
using GameFramework.DataTable;
using GameMain;
using System.Data;

namespace GameMain
{
    public class SaveLoadComponent : GameFrameworkComponent
    {
        //规定，其中0为自动存档、1~4为玩家手动存档的位置
        private GameData mGameData = new GameData();
        //初始化数据
        public GameState gameState;
        public int maxEnergy = 80;
        public int energy = 80;
        public int maxAp = 6;
        public int ap = 6;
        public int money = 3000;
        public int favor = 0;
        public int day = 0;
        public int rent = 0;
        public int closet = 1001;
        public List<ItemTag> playerItems = new List<ItemTag>();
        public void AddCGFlag(string cgTag)
        {
            if (!mGameData.cgFlags.Contains(cgTag))
                mGameData.cgFlags.Add(cgTag);
        }

        public bool ContainsCGFlag(string cgTag) => mGameData.cgFlags.Contains(cgTag);
        public float Voice
        {
            get
            {
                return mGameData.voice;
            }
            set
            {
                mGameData.voice = value;
            }
        }
        public float Word
        {
            get
            {
                return mGameData.word;
            }
            set
            {
                mGameData.word = value;
            }
        }

        public void LoadData()
        {
            GameEntry.Utils.ClearFriendFavor();
            CharSO[] charSOs = Resources.LoadAll<CharSO>("CharData");
        }

        /// <summary>
        /// 初始化游戏（测试）
        /// </summary>
        public void InitData()
        {
            LoadData();
            GameEntry.Player.MaxEnergy = maxEnergy;
            GameEntry.Player.Energy = energy;
            GameEntry.Player.MaxAp = maxAp;
            GameEntry.Player.Ap = ap;
            GameEntry.Player.Money = money;
            GameEntry.Cat.Favor = favor;
            GameEntry.Player.Day = day;
            GameEntry.Player.Rent = rent;
            GameEntry.Cat.Closet = closet;
            GameEntry.Utils.ClearFlag();
            GameEntry.Dialog.LoadGame();
            GameEntry.Level.LoadGame();
            GameEntry.Player.ClearPlayerItem();
            for (int i = 0; i < playerItems.Count; i++)
                GameEntry.Player.AddPlayerItem(new ItemData(playerItems[i]), 5);

            GameEntry.Player.AddPlayerItem(new ItemData(ItemTag.Heater), 1, true);
            GameEntry.Player.AddPlayerItem(new ItemData(ItemTag.ManualGrinder), 1, true);
            GameEntry.Player.AddPlayerItem(new ItemData(ItemTag.Kettle), 1, true);
            GameEntry.Player.AddPlayerItem(new ItemData(ItemTag.Stirrer), 1, true);
            GameEntry.Player.AddPlayerItem(new ItemData((ItemTag)closet), 1, true);

            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(gameState));

            GameEntry.Player.ClearRecipes();
            DRRecipe[] drrecipes = GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows();
            for (int i = 0; i < drrecipes.Length; i++)
            {
                if (!drrecipes[i].IsCoffee)
                    GameEntry.Player.AddRecipe(drrecipes[i].Id);
            }
            GameEntry.Buff.InitBuff();
            GameEntry.Player.RemoveRecipe(1);
            GameEntry.Player.AddRecipes(new int[] { 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219 });
        }
        public void SaveGame(int index)
        {
            SaveLoadData saveLoadData = new SaveLoadData();
            GameEntry.Event.FireNow(this, SaveGameEventArgs.Create(saveLoadData));
            DateTime dateTime = DateTime.Now;
            saveLoadData.dataTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            saveLoadData.playerData = GameEntry.Player.GetSaveData();
            saveLoadData.charData = GameEntry.Cat.GetSaveData();
            saveLoadData.utilsData = GameEntry.Utils.GetSaveData();
            saveLoadData.storyData = GameEntry.Dialog.LoadedStories;
            saveLoadData.levelData = GameEntry.Level.LoadedLevels;
            saveLoadData.buffData = GameEntry.Buff.GetSaveData();
            mGameData.saveLoadData[index] = saveLoadData;
            SaveGame();
        }

        private void SaveGame()
        {
            FileStream fs = File.Create(Application.persistentDataPath + "/save.sav");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, mGameData);
            fs.Close();
        }

        public SaveLoadData LoadGame(int index)
        {
            LoadGame();
            if (mGameData.saveLoadData[index] == null)
            {
                Debug.LogWarning("错误，不存在该存档文件");
                return null;
            }
            SaveLoadData saveLoadData = mGameData.saveLoadData[index];
            GameEntry.Event.FireNow(this, LoadGameEventArgs.Create(saveLoadData));
            return saveLoadData;
        }
        public void LoadGame()
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(Application.persistentDataPath + "/save.sav"))
                {
                    //正常开流并关闭
                    fs = File.OpenRead(Application.persistentDataPath + "/save.sav");
                    BinaryFormatter bf = new BinaryFormatter();
                    GameData gameData = (GameData)bf.Deserialize(fs);
                    mGameData = gameData;
                    fs.Close();
                }
                else
                {
                    //不存在则创建一个存档
                    mGameData = new GameData();
                }
            }
            catch (Exception e)
            {
                //无法读取则重置存档
                GameEntry.UI.OpenUIForm(UIFormId.OkTips, "<size=48>错误</size>\n存档无法读取，已经重置了您的存档");
                mGameData = new GameData();
                Debug.LogWarning(e.ToString());
            }
            finally
            {
                try
                {
                    fs.Close();
                }
                catch (Exception e) { }
                SaveGame();
            }
        }
        public void RemoveGame(int index)
        {
            mGameData.saveLoadData[index] = null;
            SaveGame();
        }
    }
    /// <summary>
    /// 需要保存的数据
    /// </summary>
    [System.Serializable]
    public class SaveLoadData
    {
        public string dataTime;
        public CatData charData;
        public PlayerData playerData;
        public UtilsData utilsData;
        public BuffData buffData = new BuffData();
        public List<string> storyData = new List<string>();
        public List<string> levelData = new List<string>();
        public List<WorkData> workDatas= new List<WorkData>();
    }
    [System.Serializable]
    public class GameData
    {
        public float voice=1f;
        public float word;
        public List<string> cgFlags = new List<string>();
        public SaveLoadData[] saveLoadData = new SaveLoadData[6];

        public GameData() { }
        public GameData(SaveLoadData[] saveLoadData) 
        { 
            this.saveLoadData = saveLoadData;
        }
    }
}
