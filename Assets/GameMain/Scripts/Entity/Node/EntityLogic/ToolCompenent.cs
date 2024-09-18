using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XNode.Examples.RuntimeMathNodes;

namespace GameMain
{
    //固定工具卡
    public class ToolCompenent : BaseCompenent
    {
        protected override void OnShow(object userData)
        {
            mHoldSprite.transform.localScale = Vector3.zero;

            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;

            mDRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mNodeData.NodeTag);

            Lock = false;
            Grind = mNodeData.Grind;
            Ice = mDRNode.Ice;
            Tool = mDRNode.Tool;

            if (Tool)
            {
                Lock = true;
                mRigidbody2D.bodyType = RigidbodyType2D.Static;
            }

            UpdateIcon();

            Producing = false;
            mIconSprite.gameObject.SetActive(false);
            mBackgroundSprite.sprite= Resources.Load<Sprite>(mDRNode.BackgroundPath);
            mBoxCollider2D.size = mBackgroundSprite.size;
            mBoundSprite.gameObject.SetActive(false);
            mTextText.gameObject.SetActive(false);
            mProgressBarRenderer.gameObject.SetActive(false);
            mCoverSprite.sprite = Resources.Load<Sprite>(mDRNode.CoverPath);
            mShaderSprite.gameObject.SetActive(false);

            mProgressBarRenderer.transform.localPosition = Vector3.down * 1.5f;
            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
            this.transform.position = mNodeData.Position;
            //处理特殊情况
            if (mNodeData.RamdonJump)
            {
                Vector3 newPos = UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * 2f, 0.5f).SetEase(Ease.OutExpo);
            }
            if (mNodeData.FirstFollow)
            {
                mCoverSprite.gameObject.SetActive(true);
                ExecuteEvents.Execute<IPointerDownHandler>(this.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                Vector3 newPos = -(mNodeData.Position - Vector3.down * 4.2f).normalized;
            }
            if (mNodeData.Adsorb != null)
            {
                Parent = mNodeData.Adsorb;
                mNodeData.Adsorb.Child = this;
            }
        }
        protected void HideChildren()
        {
            List<BaseCompenent> mMaterialBaseCompenet = new List<BaseCompenent>();
            BaseCompenent child = Child;
            while (child != null)
            {
                mMaterialBaseCompenet.Add(child);
                child = child.Child;
            }
            for (int i = 0; i < mMaterialBaseCompenet.Count; i++)
            {
                mMaterialBaseCompenet[i].gameObject.SetActive(false);
            }
            this.Child = null;
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            //刷新子集
            mChildMaterials = GenerateMaterialList();
            Compound();
        }
        protected override void Compound()
        {
            //层级刷新
            mProgressBarRenderer.GetComponent<Canvas>().sortingOrder = mIconSprite.sortingOrder + 1;
            mProgressBarRenderer.GetComponent<Canvas>().sortingLayerName = mIconSprite.sortingLayerName;
            //如果不在制作中，开始检查是否开始制作
            if (!Producing)
            {
                //开始筛选配方
                foreach (DRRecipe recipe in GameEntry.DataTable.GetDataTable<DRRecipe>().GetAllDataRows())
                {
                    if (!GameEntry.Player.HasRecipe(recipe.Id))
                        continue;

                    mRecipeData = new RecipeData(recipe);
                    if (Parent == null && Child != null)
                    {
                        if (NodeTag == mRecipeData.tool)
                        {
                            //比较逻辑
                            if (CheckList<NodeTag>(mRecipeData.materials, mChildMaterials))
                            {
                                Producing = true;
                                float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                                mProducingTime = recipe.ProducingTime * power;
                                mTime = recipe.ProducingTime * power;
                                mProgressBarRenderer.gameObject.SetActive(true);
                                HideChildren();//直接隐藏所有的子项目
                                return;
                            }
                        }
                    }
                }
                if (!Producing&&Child!=null)
                {
                    BaseCompenent baseCompenent = Child;
                    Child = null;
                    baseCompenent.Parent = null;
                    baseCompenent.transform.DOMove(mNodeData.Position + Vector3.down*3f, 0.5f).SetEase(Ease.OutExpo);
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "这个器械没办法处理这个材料");
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
