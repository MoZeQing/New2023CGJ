using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;

namespace GameMain
{
    public class MapForm : BaseForm
    {
        [SerializeField] private Button glassBtn;
        [SerializeField] private Button glothingBtn;
        [SerializeField] private Button restaurantBtn;
        [SerializeField] private Button gymBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));

            glassBtn.onClick.AddListener(() => Outing(OutingSceneState.Glass));
            restaurantBtn.onClick.AddListener(() => Outing(OutingSceneState.Restaurant));
            glothingBtn.onClick.AddListener(() => Outing(OutingSceneState.Clothing));
            gymBtn.onClick.AddListener(() => Outing(OutingSceneState.Gym));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            glassBtn.onClick.RemoveAllListeners();
            restaurantBtn.onClick.RemoveAllListeners();
            glothingBtn.onClick.RemoveAllListeners();
            gymBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void Outing(OutingSceneState outingSceneState)
        {
            GameEntry.Utils.Location=outingSceneState;
            GameEntry.Dialog.StoryUpdate();
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm((UIFormId)outingSceneState + 20, this);
            GameEntry.Event.FireNow(this, OutEventArgs.Create(outingSceneState));
        }
    }
}
