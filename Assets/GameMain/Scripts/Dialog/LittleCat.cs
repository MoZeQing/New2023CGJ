using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace GameMain
{
    public class LittleCat : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private List<SpriteRenderer> mLittleCats1 = new List<SpriteRenderer>();
        [SerializeField] private List<SpriteRenderer> mLittleCats2 = new List<SpriteRenderer>();
        [SerializeField] private List<SpriteRenderer> mLittleCats3 = new List<SpriteRenderer>();
        [SerializeField] private TeachingForm mTeachingForm;

        [SerializeField] private float switchTime=10f;

        private float nowtime;
        private int index;
        private bool autoEnable=true;

        private List<SpriteRenderer> mLittleCats = new List<SpriteRenderer>();

        private void Start()
        {
            switch (GameEntry.Utils.closet - 100)
            {
                case 1:
                    mLittleCats = mLittleCats1;
                    break;
                case 2:
                    mLittleCats= mLittleCats2;
                    break; 
                case 3:
                    mLittleCats = mLittleCats3;
                    break;
            }
            ShowLittleCat();
        }
        private void OnEnable()
        {
            //GameEntry.Event.Subscribe(MainStateEventArgs.EventId, GameStateEvent);
        }
        private void Update()
        {
            if (!autoEnable)
                return;
            nowtime -= Time.deltaTime;
            if (nowtime <= 0)
            {
                nowtime = switchTime;
                ShowLittleCat();
            }

        }
        private void OnDisable()
        {
            //GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, GameStateEvent);
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            //显示对应养成的UI
            mTeachingForm.Click_Action();
        }
        public void ShowLittleCat()
        {
            HideLittleCat();
            autoEnable = true;
            switch (GameEntry.Utils.closet - 100)
            {
                case 1:
                    mLittleCats = mLittleCats1;
                    break;
                case 2:
                    mLittleCats = mLittleCats2;
                    break;
                case 3:
                    mLittleCats = mLittleCats3;
                    break;
            }
            int block = 100;
            int newIndex = Random.Range(0, mLittleCats.Count);
            while (newIndex == index)
            {
                newIndex = Random.Range(0, mLittleCats.Count);
                block--;
                if (block < 0)
                    break;
            }
            index = newIndex;
            mLittleCats[index].DOPause();
            mLittleCats[index].GetComponent<BoxCollider2D>().enabled = true;
            mLittleCats[index].DOColor(Color.white, 0.5f);
        }
        public void HideLittleCat()
        {
            autoEnable= false ;
            foreach (SpriteRenderer sprite in mLittleCats)
            {
                sprite.GetComponent<BoxCollider2D>().enabled = false;
                sprite.DOPause();
                sprite.DOColor(Color.clear, 0.5f);
            }
        }

        //private void GameStateEvent(object sender, GameEventArgs e)
        //{
        //    MainStateEventArgs args = (MainStateEventArgs)e;
        //    if (args.MainState == MainState.Teach)
        //        ShowLittleCat();
        //    else
        //        HideLittleCat();
        //}
    }
}
