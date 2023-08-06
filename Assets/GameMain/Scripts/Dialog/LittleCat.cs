using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class LittleCat : MonoBehaviour,IPointerClickHandler
    {
        public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        public SceneTag sceneTag;

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            //显示对应养成的UI
            GameEntry.UI.OpenUIForm(UIFormId.TeachingForm, sceneTag);
            this.gameObject.SetActive(false);
        }
    }

    public enum SceneTag
    { 
        Teaching,//养成
        Working,//工作
        Cupborad//仓库
    }
}
