using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dialog
{
    public class BaseBackground : MonoBehaviour
    {
        [SerializeField] protected Image mImage;
        [SerializeField] protected Image mMask;

        public virtual void SetBackground(BackgroundData backgroundData,MyDailogBox myDailogBox)
        {
            switch (backgroundData.backgroundTag)
            {
                case BackgroundTag.None:
                    NoneState(backgroundData, myDailogBox);
                    break;
                case BackgroundTag.Fade:
                    FadeState(backgroundData, myDailogBox);
                    break;
            }
        }

        public virtual void NoneState(BackgroundData backgroundData, MyDailogBox myDailogBox) 
        {
            if(backgroundData.backgroundSpr!=null)
                mImage.sprite= backgroundData.backgroundSpr;
            myDailogBox.IsBackground = false;
        }

        public virtual void FadeState(BackgroundData backgroundData, MyDailogBox myDailogBox)
        {
            if (backgroundData.backgroundSpr != null)
            {
                mMask.sprite = mImage.sprite;
                mImage.sprite = backgroundData.backgroundSpr;
                mMask.color = Color.white;
                myDailogBox.gameObject.SetActive(false);
                mMask.DOFade(0, backgroundData.parmOne).OnComplete(() =>
                {
                    myDailogBox.gameObject.SetActive(true);
                    myDailogBox.IsBackground = false;
                    myDailogBox.Next();
                });
            }
            else
            {
                Debug.LogError("错误，存在未填写或引用错误的背景图片");
                myDailogBox.gameObject.SetActive(true);
                myDailogBox.IsBackground = false;
                myDailogBox.Next();
            }
        }
    }
}
