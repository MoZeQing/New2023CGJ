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
        public int mood = 0;
        public int favor = 0;
        public int love = 0;
        public int family= 0;
        public int day = 0;
        public int rent = 0;
        public int closet = 101;
        public List<int> recipes = new List<int>();
        public List<string> clearFlags;
        public List<ItemTag> playerItems = new List<ItemTag>();
        public List<ItemTag> greengrocerItemDatas=new List<ItemTag>();
        public List<ItemTag> bookstoreItemDatas = new List<ItemTag>();
        public List<ItemTag> musicHallItemDatas = new List<ItemTag>();
        public List<ItemTag> glassItemDatas = new List<ItemTag>();
        public List<ItemTag> restaurantItemDatas = new List<ItemTag>();
        public List<ItemTag> bakeryItemDatas = new List<ItemTag>();
        //初始化数据


        private void Start()
        {
            LoadGame();
        }

        public void LoadData()
        {
            GameEntry.Utils.recipes.Clear();
            foreach (DRRecipe recipe in GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows())
            {
                GameEntry.Utils.recipes.Add(recipe.Id.ToString(), new RecipeData(recipe));
            }

            GameEntry.Utils.chars.Clear();
            GameEntry.Utils.ClearFriendFavor();
            CharSO[] charSOs = Resources.LoadAll<CharSO>("CharData");
            for (int i = 0; i < charSOs.Length; i++)
                if (charSOs[i].friend)
                    GameEntry.Utils.chars.Add(charSOs[i].name, charSOs[i]);


        }

        /// <summary>
        /// 初始化游戏（测试）
        /// </summary>
        public void InitData()
        {
            LoadData();
            GameEntry.Utils.MaxEnergy = maxEnergy;
            GameEntry.Utils.Energy = energy;
            GameEntry.Utils.MaxAp = maxAp;
            GameEntry.Utils.Ap = ap;
            GameEntry.Utils.Money = money;
            GameEntry.Utils.Mood = mood;
            GameEntry.Utils.Favor = favor;
            GameEntry.Utils.Love = love;
            GameEntry.Utils.Family = family;
            GameEntry.Utils.Day = day;
            GameEntry.Utils.Rent= rent;
            GameEntry.Utils.closet = closet;
            GameEntry.Utils.PlayerData.heaterID =104;
            GameEntry.Utils.PlayerData.stirrerID = 110;
            GameEntry.Utils.PlayerData.pressID = 108;
            GameEntry.Utils.PlayerData.burnisherID = 101;
            GameEntry.Utils.ClearFlag();
            GameEntry.Dialog.LoadGame();
            GameEntry.Utils.ClearPlayerItem();
            for (int i = 0; i < playerItems.Count; i++)
                GameEntry.Utils.AddPlayerItem(new ItemData(playerItems[i]), 5);

            GameEntry.Utils.AddPlayerItem(new ItemData(ItemTag.Heater), 1, true);
            GameEntry.Utils.AddPlayerItem(new ItemData(ItemTag.ManualGrinder), 1, true);
            GameEntry.Utils.AddPlayerItem(new ItemData(ItemTag.Kettle), 1, true);
            GameEntry.Utils.AddPlayerItem(new ItemData(ItemTag.Stirrer), 1, true);
            GameEntry.Utils.AddPlayerItem(new ItemData((ItemTag)closet), 1, true);

            GameEntry.Utils.outingSceneStates.Clear();
            GameEntry.Utils.outingSceneStates.Add(OutingSceneState.Greengrocer);

            foreach (KeyValuePair<string, CharSO> pair in GameEntry.Utils.chars)
                GameEntry.Utils.AddFriendFavor(pair.Value.name, pair.Value.favor);

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
            GameEntry.Player.AddRecipes(new int[] { 1000,1001,1002,1003,1004,1005 });
        }
        public void SaveGame(int index)
        {
            SaveLoadData saveLoadData = new SaveLoadData();
            GameEntry.Event.FireNow(this, SaveGameEventArgs.Create(saveLoadData));
            DateTime dateTime= DateTime.Now;
            saveLoadData.dataTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            saveLoadData.day = GameEntry.Utils.Day;
            saveLoadData.closet = GameEntry.Utils.closet;
            saveLoadData.playerData = GameEntry.Utils.PlayerData;
            saveLoadData.charData= GameEntry.Utils.CharData;
            saveLoadData.flags= GameEntry.Utils.Flags;
            saveLoadData.friends = GameEntry.Utils._friends;
            saveLoadData.workDatas = GameEntry.Utils.WorkDatas;
            saveLoadData.storyData = GameEntry.Dialog.LoadedStories;
            saveLoadData.levelData = GameEntry.Dialog.LoadedLevels;
            saveLoadData.flagValue = GameEntry.Utils._flagValues;
            saveLoadData.buffs=GameEntry.Buff.GetData();
            foreach (RecipeData recipeData in GameEntry.Player.recipes)
            {
                saveLoadData.recipes.Add(recipeData.Id);
            }
            foreach (OutingSceneState outingSceneState in GameEntry.Utils.outingSceneStates)
            {
                saveLoadData.outingSceneStates.Add((int)outingSceneState);
            }
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
        public int day;
        public int closet;
        public CharData charData;
        public PlayerData playerData;
        public List<int> buffs = new List<int>();
        public List<string> storyData = new List<string>();
        public List<string> levelData = new List<string>();
        public List<WorkData> workDatas= new List<WorkData>();
        public List<string> flags=  new List<string>();
        public Dictionary<string, int> flagValue = new Dictionary<string, int>();
        public Dictionary<string, int> friends = new Dictionary<string, int>();//好友字典
        public List<int> recipes = new List<int>();
        public List<int> outingSceneStates = new List<int>();
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
