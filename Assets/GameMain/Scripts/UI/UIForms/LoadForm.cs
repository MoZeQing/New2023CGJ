using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LoadForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;

        [SerializeField] private Button loadBtn;
        [SerializeField] private Text dayText;
        [SerializeField] private Text systemText;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            loadBtn.onClick.AddListener(LoadGame);
            LoadData();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }

        private void LoadData()
        {
            SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(0);
            if (saveLoadData == null) return;
            dayText.text = string.Format("µÚ{0}Ìì", saveLoadData.day);
            systemText.text = saveLoadData.dataTime;
        }

        private void LoadGame()
        {
            SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(0);
            GameEntry.Utils.PlayerData=saveLoadData.playerData;
            GameEntry.Utils.CharData=saveLoadData.charData;
            GameEntry.Utils.Day=saveLoadData.day;
            GameEntry.Utils.Flags=saveLoadData.flags;
            GameEntry.Utils.WorkDatas=saveLoadData.workDatas;
            GameEntry.Dialog.LoadGame(saveLoadData.storyData);
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Teach));
        }
    }
}
