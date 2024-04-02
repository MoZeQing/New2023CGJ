using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class GuideForm : UIFormLogic
    {
        [SerializeField] private Button add;
        [SerializeField] private Button reduce;
        [SerializeField] private Text indexText;
        [SerializeField] private Image image;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private List<Button> buttons = new List<Button>();
        [SerializeField] private List<Sprite> dialogs1=new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs2=new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs3=new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs4 = new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs5 = new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs6 = new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs7 = new List<Sprite>();
        [SerializeField] private List<Sprite> dialogs8 = new List<Sprite>();

        private List<Sprite> mDialogs = new List<Sprite>();
        private int index;
        private ProcedureGuide mProcedureGuide;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            OnClick(0);
            add.onClick.AddListener(OnAdd);
            reduce.onClick.AddListener(OnReduce);
            buttons[0].onClick.AddListener(()=>OnClick(0));
            buttons[1].onClick.AddListener(() => OnClick(1));
            buttons[2].onClick.AddListener(() => OnClick(2));
            buttons[3].onClick.AddListener(() => OnClick(3));
            buttons[4].onClick.AddListener(() => OnClick(4));
            buttons[5].onClick.AddListener(() => OnClick(5));
            buttons[6].onClick.AddListener(() => OnClick(6));
            buttons[7].onClick.AddListener(() => OnClick(7));
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            add.onClick.RemoveAllListeners();
            reduce.onClick.RemoveAllListeners();
            buttons[0].onClick.RemoveAllListeners();
            buttons[1].onClick.RemoveAllListeners();
            buttons[2].onClick.RemoveAllListeners();
            buttons[3].onClick.RemoveAllListeners();
            buttons[4].onClick.RemoveAllListeners();
            buttons[5].onClick.RemoveAllListeners();
            buttons[6].onClick.RemoveAllListeners();
            buttons[7].onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void OnClick(int i)
        {
            index = 0;
            switch (i)
            {
                case 0:mDialogs = dialogs1;break;
                case 1:mDialogs = dialogs2;break;
                case 2:mDialogs = dialogs3;break;
                case 3: mDialogs = dialogs4; break;
                case 4: mDialogs = dialogs5; break;
                case 5: mDialogs = dialogs6; break;
                case 6: mDialogs = dialogs7; break;
                case 7: mDialogs = dialogs8; break;
            }
            image.sprite = mDialogs[index];
            indexText.text = string.Format("{0}/{1}", index + 1, mDialogs.Count);
        }

        private void OnAdd()
        {
            index++;
            if (index >= mDialogs.Count)
            {
                index = 0;
            }
            image.sprite = mDialogs[index];
            indexText.text = string.Format("{0}/{1}", index + 1, mDialogs.Count);
        }

        private void OnReduce()
        {
            index--;
            if (index < 0)
            {
                index = mDialogs.Count - 1;
            }
            image.sprite = mDialogs[index];
            indexText.text = string.Format("{0}/{1}", index + 1, mDialogs.Count);
        }
    }
}
