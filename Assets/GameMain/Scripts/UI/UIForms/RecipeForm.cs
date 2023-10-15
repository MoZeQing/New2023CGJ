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
        [SerializeField] private GameObject mNodeItem;
        [SerializeField] private Image mLeftImage;
        [SerializeField] private Text mIndexText;
        [SerializeField] private Transform mRightCanvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button mLeftBtn;
        [SerializeField] private Button mRightBtn;
        [SerializeField] private List<List<Sprite>> nodeSprites = new List<List<Sprite>>();

        private List<RecipeItem> nodeItems= new List<RecipeItem>();
        private int mIndex = 0;
        private NodeTag mNodeTag;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            mLeftBtn.onClick.AddListener(Left);
            mRightBtn.onClick.AddListener(Right);

            ShowNodes();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            mLeftBtn.onClick.RemoveAllListeners();
            mRightBtn.onClick.RemoveAllListeners();

            ClearNodes();
        }

        private void ShowNodes()
        {
            ClearNodes();
            for (int i = 0; i < 35; i++)
            {
                GameObject go = Instantiate(mNodeItem, mRightCanvas);
                RecipeItem recipe=go.GetComponent<RecipeItem>();
                recipe.SetData((NodeTag)i, ShowRecipe);
                nodeItems.Add(recipe);
            }
        }

        private void ClearNodes()
        {
            foreach (RecipeItem go in nodeItems)
            { 
                Destroy(go.gameObject);
            }
        }

        private void ShowRecipe(NodeTag nodeTag)
        {
            mNodeTag= nodeTag;
            mIndex = 0;
            mLeftImage.sprite = nodeSprites[(int)mNodeTag][mIndex];
            mIndexText.text = string.Format("{0}/{1}", mIndex + 1, nodeSprites[(int)mNodeTag].Count);
        }

        private void Left()
        {
            mIndex--;
            if (mIndex < 0)
                mIndex = nodeSprites[(int)mNodeTag].Count - 1;
            mLeftImage.sprite = nodeSprites[(int)mNodeTag][mIndex];
            mIndexText.text = string.Format("{0}/{1}", mIndex + 1, nodeSprites[(int)mNodeTag].Count);
        }

        private void Right()
        {
            mIndex++;
            if (mIndex >= nodeSprites[(int)mNodeTag].Count)
                mIndex = 0;
            mLeftImage.sprite = nodeSprites[(int)mNodeTag][mIndex];
            mIndexText.text = string.Format("{0}/{1}", mIndex + 1, nodeSprites[(int)mNodeTag].Count);
        }
    }
}
