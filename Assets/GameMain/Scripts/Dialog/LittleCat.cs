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
        [SerializeField] private ScenePosTag mSceneTag;
        [SerializeField] private TeachingForm mTeachingForm;

        private void Start()
        {
            mLittleCats.Clear();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                mLittleCats.Add(this.transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
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
        public void ShowLittleCat(ScenePosTag posTag)
        {
            HideLittleCat();
            mSceneTag= posTag;
            mLittleCats[(int)mSceneTag].gameObject.SetActive(true);
        }
        public void ShowLittleCat()
        {
            HideLittleCat();
            mLittleCats[(int)mSceneTag].gameObject.SetActive(true);
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

    public enum ScenePosTag
    { 
        Left,//左
        Middle,//中
        Right//右
    }
}
