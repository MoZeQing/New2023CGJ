using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XNode.Examples.RuntimeMathNodes;

namespace GameMain
{
    public class PressCompenent : ToolCompenent, IPointerClickHandler
    {
        protected NodeTag coffeeBean = NodeTag.None;
        protected NodeTag water=NodeTag.None;
        protected override void Compound()
        {
            //������������У���ʼ����Ƿ�ʼ����
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
                    mAnimator.SetBool("Producing", true);
                    Producing = true;
                    float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                    mProducingTime = recipe.ProducingTime * power;
                    mTime = recipe.ProducingTime * power;
                    mBackgroundSprite.sprite = Resources.Load<Sprite>("Image/Card/press_anim");
                    mProgressBarRenderer.gameObject.SetActive(true);
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
                    GameEntry.Event.FireNow(this, WorkEventArgs.Create("���ΙCе�ǤϤ��β��Ϥ�I��Ǥ��ޤ���", WorkTips.Tips));
                }
            }
            else//�������������
            {
                mProgressBarRenderer.gameObject.SetActive(true);
                mProgressBarRenderer.fillAmount = 1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (mProducingTime <= 0)//����������
                {
                    if (Child != null)//ɾ��ȫ�����ӽڵ�
                    {
                        RemoveChildren();
                    }//�������Ʒ���в���
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
                    mAnimator.SetBool("Producing", false);
                    mBackgroundSprite.sprite = Resources.Load<Sprite>(mDRNode.BackgroundPath);
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipeData = null;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    Producing = false;
                    coffeeBean = NodeTag.None;
                    water = NodeTag.None;
                    return;
                }
            }
        }
        public new void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                mAnimator.SetBool("Producing", false);
                mBackgroundSprite.sprite = Resources.Load<Sprite>(mDRNode.BackgroundPath);
                mProducingTime = 0;
                mTime = 0f;
                mRecipeData = null;
                mProgressBarRenderer.gameObject.SetActive(false);
                Producing = false;
                if (coffeeBean != NodeTag.None)
                {
                    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, coffeeBean)
                    {
                        Position = this.transform.position + new Vector3(0.5f, 0, 0),
                        RamdonJump = true,
                    });
                    coffeeBean = NodeTag.None;
                }
                if (water != NodeTag.None)
                {
                    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, water)
                    {
                        Position = this.transform.position + new Vector3(0.5f, 0, 0),
                        RamdonJump = true,
                    });
                    water = NodeTag.None;
                }
            }
        }
    }
}

