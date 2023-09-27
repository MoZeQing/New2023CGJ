using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices.ComTypes;

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
            saveLoadData.closet = GameEntry.Utils.closet;
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
