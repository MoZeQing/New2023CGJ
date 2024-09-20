using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class PressCompenent : ToolCompenent
    {
        protected NodeTag coffeeBean = NodeTag.None;
        protected NodeTag water=NodeTag.None;
        protected override void Compound()
        {
            //如果不在制作中，开始检查是否开始制作
            if (!Producing)
            {
                if (Parent == null && Child != null)
                {
                    if (Child.NodeTag == NodeTag.CoarseGroundCoffee)
                    {
                        coffeeBean = NodeTag.CoarseGroundCoffee;
                        mBackgroundSprite.sprite = Resources.Load<Sprite>("Image/Card/press_anim_2");
                        HideChildren();
                    }
                    else if (Child.NodeTag == NodeTag.FineGroundCoffee)
                    {
                        coffeeBean = NodeTag.FineGroundCoffee;
                        mBackgroundSprite.sprite = Resources.Load<Sprite>("Image/Card/press_anim_2");
                        HideChildren();
                    }
                    else if (Child.NodeTag == NodeTag.HotWater)
                    {
                        water = NodeTag.HotWater;
                        mBackgroundSprite.sprite = Resources.Load<Sprite>("Image/Card/press_anim_1");
                        HideChildren();
                    }
                }
                if (coffeeBean != NodeTag.None && water != NodeTag.None)
                {
                    DRRecipe recipe = null;
                    if (coffeeBean == NodeTag.CoarseGroundCoffee)
                    {
                        recipe = GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(18);
                        mRecipeData = new RecipeData(recipe);
                    }
                    if (coffeeBean == NodeTag.FineGroundCoffee)
                    {
                        recipe = GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(22);
                        mRecipeData = new RecipeData(recipe);
                    }
                    Producing = true;
                    float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                    mProducingTime = recipe.ProducingTime * power;
                    mTime = recipe.ProducingTime * power;
                    mBackgroundSprite.sprite = Resources.Load<Sprite>("Image/Card/press_anim");
                    mProgressBarRenderer.gameObject.SetActive(true);
                    coffeeBean = NodeTag.None;
                    water = NodeTag.None;
                }
                if (!Producing && Child != null)
                {
                    BaseCompenent baseCompenent = Child;
                    baseCompenent.Parent = null;
                    baseCompenent.BestCompenent = null;
                    Child = null;
                    this.mBoxCollider2D.enabled = false;
                    baseCompenent.transform.DOMove(mNodeData.Position + Vector3.down * 3f, 0.5f).SetEase(Ease.OutExpo)
                        .OnComplete(() => this.mBoxCollider2D.enabled = true);
                    GameEntry.Event.FireNow(this, WorkEventArgs.Create("这个器械没办法处理这个材料", WorkTips.Tips));
                }
            }
            else//如果正在制作中
            {
                mProgressBarRenderer.gameObject.SetActive(true);
                mProgressBarRenderer.fillAmount = 1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (mProducingTime <= 0)//如果完成制作
                {
                    if (Child != null)//删除全部的子节点
                    {
                        RemoveChildren();
                    }//根据完成品进行产出
                    for (int i = 0; i < mRecipeData.products.Count; i++)
                    {
                        if (mRecipeData.products[i] == NodeTag.EspressoG)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Espresso)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = true
                            });
                        }
                        else
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mRecipeData.products[i])
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true,
                                Grind = GetChildGrind()
                            });
                        }
                    }
                    if (this.NodeTag == NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    mBackgroundSprite.sprite = Resources.Load<Sprite>(mDRNode.BackgroundPath);
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

