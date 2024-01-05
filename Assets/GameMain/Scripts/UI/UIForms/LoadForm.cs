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
        [Header("快速/自动读档")]
        [SerializeField] private Button loadBtn;
        [SerializeField] private Text dayText;
        [SerializeField] private Text systemText;

        [SerializeField] private SaveLoadItem[] saveLoadItems=new SaveLoadItem[4];

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            loadBtn.onClick.AddListener(() => LoadGame(0));
            LoadData();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
        }

        private void LoadData()
        {
            SaveLoadData autoLoadData = GameEntry.SaveLoad.LoadGame(0);
            if (autoLoadData == null) return;
            dayText.text = string.Format("第{0}天", autoLoadData.day);
            systemText.text = autoLoadData.dataTime;
            for (int i = 1; i < 5; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                    Debug.Log("不存在该存档");
                else
                    saveLoadItems[i - 1].SetData(saveLoadData, LoadGame, i);
            }
        }

        private void LoadGame(int index)
        {
            SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(index);
            GameEntry.SaveLoad.InitData();
            GameEntry.Utils.PlayerData = saveLoadData.playerData;
            GameEntry.Utils.CharData = saveLoadData.charData;
            GameEntry.Utils.Day = saveLoadData.day;
            GameEntry.Utils.closet = saveLoadData.closet;
            GameEntry.Utils.Flags = saveLoadData.flags;
            GameEntry.Utils.WorkDatas = saveLoadData.workDatas;
            GameEntry.Dialog.LoadGame(saveLoadData.storyData);
            GameEntry.Player.LoadGame(saveLoadData);
            GameEntry.Utils.outingSceneStates.Clear();
            foreach (OutingSceneState outingSceneState in saveLoadData.outingSceneStates)
            {
                GameEntry.Utils.outingSceneStates.Add(outingSceneState);
            }
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
            GameEntry.SaveLoad.LoadData();
            LoadData();
        }
    }
}
