using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LittleCat : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private List<SpriteRenderer> mLittleCats1 = new List<SpriteRenderer>();
        [SerializeField] private List<SpriteRenderer> mLittleCats2 = new List<SpriteRenderer>();
        [SerializeField] private List<SpriteRenderer> mLittleCats3 = new List<SpriteRenderer>();
        [SerializeField] private TeachingForm mTeachingForm;

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
            mLittleCats[Random.Range(0, mLittleCats.Count)].gameObject.SetActive(true);
        }
        public void HideLittleCat()
        {
            foreach (SpriteRenderer sprite in mLittleCats)
            { 
                sprite.gameObject.SetActive(false);
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
