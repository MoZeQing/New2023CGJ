using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LittleCat : Entity,IPointerClickHandler
    {
        public List<SpriteRenderer> littleCats = new List<SpriteRenderer>();
        public ScenePosTag sceneTag;

        private TeachingForm mTeachingForm = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            LittleCharData littleCharData = (LittleCharData)userData;
            mTeachingForm = littleCharData.TeachingForm;

            littleCats.Clear();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                littleCats.Add(this.transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            //显示对应养成的UI
            mTeachingForm.Click_Action();
        }
        public void ShowLittleCat(ScenePosTag posTag)
        {
            HideLittleCat();
            sceneTag= posTag;
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void ShowLittleCat()
        {
            HideLittleCat();
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void HideLittleCat()
        {
            foreach (SpriteRenderer sprite in littleCats)
            { 
                sprite.gameObject.SetActive(false);
            }
        }
    }

    public enum ScenePosTag
    { 
        Left,//左
        Middle,//中
        Right//右
    }
}
