using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SaveForm : BaseForm
    {
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;

        [SerializeField] private SaveLoadItem[] saveLoadItems = new SaveLoadItem[6];

        private bool isLoad;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            loadBtn.onClick.AddListener(LoadData);
            saveBtn.onClick.AddListener(SaveData);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            isLoad = true;
            LoadData();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
            saveBtn.onClick.RemoveAllListeners();
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
            GameEntry.Player.ClearRecipes();
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.CloseUIForm(this.UIForm);
            LoadData();
        }

        private void SaveGame(int index)
        {
            GameEntry.SaveLoad.SaveGame(index);
            SaveData();
        }
        private void RemoveSaveData(int index)
        {
            GameEntry.SaveLoad.RemoveGame(index);
            SaveData();
        }
        private void RemoveLoadData(int index)
        {
            GameEntry.SaveLoad.RemoveGame(index);
            LoadData();
        }

        private void SaveData()
        {
            loadBtn.transform.GetChild(0).gameObject.SetActive(false);
            saveBtn.transform.GetChild(0).gameObject.SetActive(true);
            isLoad = false;
            SaveLoadData autoLoadData = GameEntry.SaveLoad.LoadGame(0);
            for (int i = 0; i < saveLoadItems.Length; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                {
                    saveLoadItems[i].SetSaveData(SaveGame, i);
                }
                else
                {
                    saveLoadItems[i].SetData(saveLoadData, SaveGame, i);
                    saveLoadItems[i].SetCancel(RemoveSaveData, i);
                }
            }
        }
        private void LoadData()
        {
            loadBtn.transform.GetChild(0).gameObject.SetActive(true);
            saveBtn.transform.GetChild(0).gameObject.SetActive(false);
            isLoad = true;
            for (int i = 0; i < saveLoadItems.Length; i++)
            {
                SaveLoadData saveLoadData = GameEntry.SaveLoad.LoadGame(i);
                if (saveLoadData == null)
                    saveLoadItems[i].Hide();
                else
                {
                    saveLoadItems[i].SetData(saveLoadData, LoadGame, i);
                    saveLoadItems[i].SetCancel(RemoveLoadData, i);
                }
            }
        }


    }
}


