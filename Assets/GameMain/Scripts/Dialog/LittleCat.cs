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
        [SerializeField] private List<SpriteRenderer> mLittleCats = new List<SpriteRenderer>();
        [SerializeField] private TeachingForm mTeachingForm;

        private void Start()
        {
            mLittleCats.Clear();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                mLittleCats.Add(this.transform.GetChild(i).GetComponent<SpriteRenderer>());
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
            mLittleCats[Random.Range(0, mLittleCats.Count - 1)].gameObject.SetActive(true);
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
