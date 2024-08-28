using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class QueryForm :BaseForm
    {
        [SerializeField] private Text queryText;
        [SerializeField] private Text[] answerTexts;
        [SerializeField] private Button[] answerBtns;
        [SerializeField] private Image[] winFailImgs;
        [SerializeField] private Image queryImg;
        [SerializeField] private Sprite winSprite;
        [SerializeField] private Sprite failSprite;
        [SerializeField] private Transform answerTitle;
        [SerializeField] private Text answerText;
        [SerializeField] private Button answerTextBtn;

        [SerializeField] private Transform canvas;
        [SerializeField] private int totalQuery;
        [SerializeField] private CatData charData;
        [SerializeField] private PlayerData playerData;

        private Action mAction;
        private int queryCount;
        private int trueCount;
        private DRQuery query;
        private List<DRQuery> queries= new List<DRQuery>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mAction = BaseFormData.Action;

            answerTitle.gameObject.SetActive(false);
            answerTextBtn.onClick.AddListener(ShowQuery);

            queries.Clear();
            queries = new List<DRQuery>(GameEntry.DataTable.GetDataTable<DRQuery>().GetAllDataRows());

            queryCount = 0;
            trueCount= 0;

            ShowQuery();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            answerTextBtn.onClick.RemoveAllListeners();
        }

        private void ShowQuery()
        {
            if (totalQuery <= queryCount)
            {
                float power = (float)trueCount / (float)totalQuery;
                int wisdom = (int)(charData.wisdom * power);
                Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
                dic.Add(ValueTag.Wisdom, wisdom);
                playerData.GetValueTag(dic);
                GameEntry.UI.OpenUIForm(UIFormId.CompleteForm,OnExit,dic);
                return;
            }
            answerTitle.gameObject.SetActive(false);
            for (int i = 0; i < answerBtns.Length; i++)
            {
                answerBtns[i].interactable = true;
            }
            for (int i = 0; i < winFailImgs.Length; i++)
            {
                winFailImgs[i].gameObject.SetActive(false);
            }
            query = queries[UnityEngine.Random.Range(0, queries.Count)];
            queryText.text = $"Q{queryCount+1}/{totalQuery}（完成的正确回答:{trueCount}）：\n{query.Query}";
            answerTexts[0].text = $"A.{query.Answer1}";
            answerTexts[1].text = $"B.{query.Answer2}";
            answerTexts[2].text = $"C.{query.Answer3}";
            answerTexts[3].text = $"D.{query.Answer4}";

            queryImg.gameObject.SetActive(query.ImagePath != string.Empty);
            if (query.ImagePath != string.Empty)
            {
                queryImg.sprite = Resources.Load<Sprite>(query.ImagePath);
            }
            answerBtns[0].onClick.RemoveAllListeners();
            answerBtns[0].onClick.AddListener(() => OnClick(1));
            answerBtns[1].onClick.RemoveAllListeners();
            answerBtns[1].onClick.AddListener(() => OnClick(2));
            answerBtns[2].onClick.RemoveAllListeners();
            answerBtns[2].onClick.AddListener(() => OnClick(3));
            answerBtns[3].onClick.RemoveAllListeners();
            answerBtns[3].onClick.AddListener(() => OnClick(4));
            queries.Remove(query);
            queryCount++;
        }

        private void OnClick(int index)
        {
            for (int i = 0; i < answerBtns.Length; i++)
            {
                answerBtns[i].interactable= false;
            }
            if (index == query.TrueAnswer)
            {
                winFailImgs[index - 1].gameObject.SetActive(true);
                winFailImgs[index - 1].sprite = winSprite;
                //GameEntry.Sound.PlaySound();
                trueCount++;
                Invoke(nameof(ShowQuery), 2f);
            }
            else
            {
                winFailImgs[index - 1].gameObject.SetActive(true);
                winFailImgs[index - 1].sprite = failSprite;
                //GameEntry.Sound.PlaySound();
                Invoke(nameof(ShowAnswer), 2f);
            }
        }

        private void ShowAnswer()
        {
            answerTitle.gameObject.SetActive(true);
            answerTitle.transform.localPosition = Vector3.up * 1080f;
            answerText.text = string.Empty;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(answerTitle.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutExpo));
            sequence.Append(answerText.DOText($"{query.AnswerTitle}\n\n点击X键进入下一题……", query.AnswerTitle.Length * 0.06f)); 
        }

        private void OnExit()
        {
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}

