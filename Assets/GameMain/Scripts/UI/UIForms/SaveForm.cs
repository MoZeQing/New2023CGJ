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
        [SerializeField] private Button loadBtn;
        [SerializeField] private Text dayText;
        [SerializeField] private Text systemText;

        [SerializeField] private SaveLoadItem[] saveLoadItems = new SaveLoadItem[4];

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            loadBtn.onClick.AddListener(() => SaveGame(0));
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
            if (autoLoadData == null) return;
            dayText.text = string.Format("第{0}天", autoLoadData.day);
            systemText.text = autoLoadData.dataTime;
            for (int i = 1; i < 5; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                    saveLoadItems[i-1].SetData(SaveGame,i);
                else
                    saveLoadItems[i-1].SetData(saveLoadData, SaveGame, i);
            }
        }

        private void SaveGame(int index)
        {
            GameEntry.SaveLoad.SaveGame(index);
            SaveData();
        }
    }
}

