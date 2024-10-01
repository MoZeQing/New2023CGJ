using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LoadForm : BaseForm
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;

        [SerializeField] private SaveLoadItem[] saveLoadItems=new SaveLoadItem[6];

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            LoadData();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
        }

        private void LoadData()
        {
            for (int i = 0; i < saveLoadItems.Length; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                    saveLoadItems[i].Hide();
                else
                {
                    saveLoadItems[i].SetData(saveLoadData, LoadGame, i);
                    saveLoadItems[i].SetCancel(RemoveData, i);
                }
            }
        }

        private void RemoveData(int index)
        {
            GameEntry.SaveLoad.RemoveGame(index);
            LoadData();
        }

        private void LoadGame(int index)
        {
            GameEntry.SaveLoad.LoadData();
            SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(index);
            GameEntry.SaveLoad.InitData();
            GameEntry.Player.LoadData(saveLoadData.playerData);
            GameEntry.Cat.LoadData(saveLoadData.charData);
            GameEntry.Utils.LoadData(saveLoadData.utilsData);
            GameEntry.Dialog.LoadGame(saveLoadData.storyData);
            GameEntry.Level.LoadGame(saveLoadData.levelData);
            GameEntry.Player.LoadGame(saveLoadData);
            GameEntry.Buff.LoadData(saveLoadData.buffData);
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.CloseUIForm(this.UIForm);
            LoadData();
        }
    }
}
