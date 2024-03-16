using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SaveForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [Header("快速/自动读档")]

        [SerializeField] private SaveLoadItem[] saveLoadItems = new SaveLoadItem[4];

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            SaveData();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }

        private void SaveData()
        {
            SaveLoadData autoLoadData = GameEntry.SaveLoad.LoadGame(0);
            for (int i = 0; i < saveLoadItems.Length; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                {
                    saveLoadItems[i].SetSaveData(SaveGame, i);
                }
                else
                    saveLoadItems[i].SetData(saveLoadData, SaveGame, i);
            }
        }

        private void SaveGame(int index)
        {
            GameEntry.SaveLoad.SaveGame(index);
            SaveData();
        }
    }
}

