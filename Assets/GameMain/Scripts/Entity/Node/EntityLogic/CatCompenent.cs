using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CatCompenent : BaseCompenent
    {
        private NodeTag productTag;
        private bool productGrind;
        protected override void Compound()
        {
            //层级刷新
            mProgressBarRenderer.GetComponent<Canvas>().sortingOrder = mIconSprite.sortingOrder + 1;
            mProgressBarRenderer.GetComponent<Canvas>().sortingLayerName = mIconSprite.sortingLayerName;
            //如果不在制作中，开始检查是否开始制作
            if (!Producing)
            {
                if (Child == null)
                    return;
                if (Child.IsCoffee)
                {
                    productTag = Child.NodeTag;
                    productGrind = !Child.Grind;
                    Producing = true;
                    float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                    mProducingTime = 10 * power;
                    mTime = 10 * power;
                    mProgressBarRenderer.gameObject.SetActive(true);
                    return;
                }
                //比较逻辑
            }
            else//如果正在制作中
            {
                mProgressBarRenderer.gameObject.SetActive(true);
                mProgressBarRenderer.fillAmount = 1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (Child==null ||!Child.IsCoffee)
                {
                    mProducingTime = 0;
                    mTime = 0f;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    mProgressBarRenderer.fillAmount = 1;
                    Producing = false;
                    return;
                }
                if (mProducingTime <= 0)//如果完成制作
                {
                    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, productTag)
                    {
                        Position = this.transform.position + new Vector3(0.5f, 0, 0),
                        RamdonJump = false,
                        Grind = productGrind
                    });
                    RemoveChildren();//删除全部的子节点
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipeData = null;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    Producing = false;
                    return;
                }
            }
        }
    }

}