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

        [SerializeField] private Transform canvas;
        [SerializeField] private int totalQuery;
        [SerializeField] private CharData charData;
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

            canvas.transform.localPosition = Vector3.up * 1080f;
            canvas.transform.DOLocalMove(Vector3.zero, 1f);
            answerTitle.gameObject.SetActive(false);

            queries.Clear();
            queries = new List<DRQuery>(GameEntry.DataTable.GetDataTable<DRQuery>().GetAllDataRows());

            queryCount = 1;
            trueCount= 0;

            ShowQuery();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void ShowQuery()
        {
            answerTitle.gameObject.SetActive(false);
            for (int i = 0; i < answerBtns.Length; i++)
            {
                answerBtns[i].interactable = true;
            }
            query = queries[UnityEngine.Random.Range(0, queries.Count)];
            queryText.text = $"Q{queryCount}£º\n{query.Query}";
            answerTexts[0].text = $"A.{query.Answer1}";
            answerTexts[1].text = $"B.{query.Answer2}";
            answerTexts[2].text = $"C.{query.Answer3}";
            answerTexts[3].text = $"D.{query.Answer4}";

            queryImg.gameObject.SetActive(query.ImagePath != string.Empty);
            if (query.ImagePath != string.Empty)
            {
                queryImg.sprite = Resources.Load<Sprite>(query.ImagePath);
            }

            answerBtns[0].onClick.AddListener(() => OnClick(1));
            answerBtns[1].onClick.AddListener(() => OnClick(2));
            answerBtns[2].onClick.AddListener(() => OnClick(3));
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
                winFailImgs[index - 1].sprite = winSprite;
                //GameEntry.Sound.PlaySound();
                trueCount++;
                Invoke(nameof(ShowQuery), 3f);
            }
            else
            {
                winFailImgs[index - 1].sprite = failSprite;
                //GameEntry.Sound.PlaySound();
                Invoke(nameof(ShowAnswer), 3f);
            }
        }

        private void ShowAnswer()
        {
            answerTitle.gameObject.SetActive(true);
            answerTitle.transform.localPosition = Vector3.up * 1080f;
            answerText.text = query.AnswerTitle;
            answerTitle.DOLocalMove(Vector3.zero, 1f);
            Invoke(nameof(ShowQuery), 3f);
        }
    }
}

