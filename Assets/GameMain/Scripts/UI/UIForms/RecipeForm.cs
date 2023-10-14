using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class RecipeForm :UIFormLogic
    {
        [SerializeField] private Button mUpBtn;
        [SerializeField] private Button mDownBtn;
        [SerializeField] private Text mNum;
        [SerializeField] private Image mImage = null;
        [SerializeField] private Button exitBtn;

        [SerializeField] private List<Sprite> mSprites = new List<Sprite>();

        private int mIndex = 0;
        // Start is called before the first frame update

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mUpBtn.onClick.AddListener(Up);
            mDownBtn.onClick.AddListener(Down);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            mNum.text = string.Format("{0}/{1}", mIndex + 1, mSprites.Count);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            mUpBtn.onClick.RemoveAllListeners();
            mDownBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void Up()
        {
            mIndex++;
            if(mIndex>=mSprites.Count)
                mIndex= 0;
            mImage.sprite = mSprites[mIndex];
            mNum.text = string.Format("{0}/{1}", mIndex + 1, mSprites.Count);
        }

        private void Down() 
        { 
            mIndex--;
            if (mIndex < 0)
                mIndex = mSprites.Count-1;
            mImage.sprite = mSprites[mIndex];
            mNum.text = string.Format("{0}/{1}", mIndex + 1, mSprites.Count);
        }
    }
}
