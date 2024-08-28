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
        private SaveLoadData[] mSaveLoadData = new SaveLoadData[6];
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
        private void Start()
        {
            LoadGame();
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
            GameEntry.Player.Rent= rent;
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

            GameEntry.Player.ClearRecipe();
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
            DateTime dateTime= DateTime.Now;
            saveLoadData.dataTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            saveLoadData.playerData = GameEntry.Player.GetSaveData();
            saveLoadData.charData = GameEntry.Cat.GetSaveData();
            saveLoadData.utilsData=GameEntry.Utils.GetSaveData();
            saveLoadData.storyData = GameEntry.Dialog.LoadedStories;
            saveLoadData.levelData = GameEntry.Level.LoadedLevels;
            saveLoadData.buffData = GameEntry.Buff.GetSaveData();
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
            try
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
            catch(Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
        public void RemoveGame(int index)
        {
            mSaveLoadData[index] = null;
            SaveGame();
        }
        //摄屏
        public Texture2D ScreenShot(Camera camera, Rect rect)
        {
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height,0);
            camera.targetTexture= rt;
            camera.Render();
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string path = Application.dataPath + "/ Resources / ScreenShot / screenshot.png";
            System.IO.File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return screenShot;
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
        public SaveLoadData[] saveLoadData = new SaveLoadData[5];

        public GameData() { }
        public GameData(SaveLoadData[] saveLoadData) 
        { 
            this.saveLoadData = saveLoadData;
        }
    }
}
