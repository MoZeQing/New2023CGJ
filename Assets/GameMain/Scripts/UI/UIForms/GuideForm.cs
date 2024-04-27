using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

namespace GameMain
{
    public class GuideForm : UIFormLogic
    {
        [SerializeField] private Text text;
        [SerializeField] private Text title;
        [SerializeField] private RawImage image;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private List<Button> buttons = new List<Button>();
        [SerializeField] private VideoPlayer videoPlayer;

        RenderTexture renderTexture;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            renderTexture = RenderTexture.GetTemporary(1920, 1080);

            image.texture = renderTexture;
            image.color = new Color(0f, 0f, 0f, 0f);
            image.enabled = true;

            videoPlayer.targetTexture = renderTexture;

            OnClick(GameEntry.DataTable.GetDataTable<DRGuide>().GetDataRow(0));
            for (int i=0;i<buttons.Count;i++)
            {
                DRGuide dRGuide = GameEntry.DataTable.GetDataTable<DRGuide>().GetDataRow(i);
                buttons[i].onClick.AddListener(() => OnClick(dRGuide));
            }
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
            exitBtn.onClick.RemoveAllListeners();
        }


        private void OnClick(DRGuide dRGuide)
        {
            videoPlayer.clip= (VideoClip)Resources.Load<VideoClip>(dRGuide.VideoPath);
            videoPlayer.Play();
            image.color = Color.white;
            title.text = dRGuide.Title;
            text.text = dRGuide.Text;
        }
    }
}
