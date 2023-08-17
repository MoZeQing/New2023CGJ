using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;


namespace GameMain
{
    public class TeachStage : Entity
    {
        public List<SpriteRenderer> littleCats = new List<SpriteRenderer>();
        public ScenePosTag sceneTag;

        private TeachingForm mTeachingForm = null;
        private LittleCharData mLittleCharData = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mLittleCharData = (LittleCharData)userData;
            mTeachingForm = mLittleCharData.TeachingForm;

            littleCats.Clear();
            littleCats.Add(this.transform.Find("Cat1").GetComponent<SpriteRenderer>());
            littleCats.Add(this.transform.Find("Cat2").GetComponent<SpriteRenderer>());
            littleCats.Add(this.transform.Find("Cat3").GetComponent<SpriteRenderer>());
            for (int i = 0; i < this.transform.childCount; i++)
            {
                littleCats.Add(this.transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }

        public void ShowLittleCharacter(ScenePosTag posTag)
        {
            HideLittleCharacter();
            sceneTag = posTag;
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void ShowLittleCharacter()
        {
            HideLittleCharacter();
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void HideLittleCharacter()
        {
            foreach (SpriteRenderer sprite in littleCats)
            {
                sprite.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// …Ë÷√∂Øª≠
        /// </summary>
        /// <param name="baseCharacter"></param>
        /// <param name="actionData"></param>
        private void SetDialogAction(ActionData actionData)
        {

        }
    }
}