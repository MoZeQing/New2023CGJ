using GameMain;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseStage : MonoBehaviour
{
    [SerializeField] protected bool IsWorld; 

    [SerializeField] protected Image mBG;
    [SerializeField] protected Image mMaskBG;
    [SerializeField] protected Transform mCanvas;
    [SerializeField] protected GameObject mCharacter;
    [SerializeField] protected List<Transform> mPositions = new List<Transform>();
    [SerializeField] protected List<BaseCharacter> mChars = new List<BaseCharacter>();
    protected Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();
    //缓存区

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="baseCharacter"></param>
    /// <param name="dialogPos"></param>
    protected virtual void SetDialogPos(BaseCharacter baseCharacter, DialogPos dialogPos)
    {
        for (int i = 0; i < mChars.Count; i++)
        {
            if (i == (int)dialogPos)
            {
                if (mChars[i] != baseCharacter && mChars[i]!=null)
                    mChars[i].gameObject.SetActive(false);
                if (mChars[i] == baseCharacter)
                    continue;
                mChars[i] = baseCharacter;
                Vector3 offset = mChars[i].mCharSO.offset;
                if (IsWorld) offset /= 100;
                mChars[i].transform.position = mPositions[(int)dialogPos].transform.position+ offset;
                mChars[i].gameObject.SetActive(true);
            }
            else
            {
                if (baseCharacter == mChars[i])
                    mChars[i] = null;
            }
        }
    }
    public virtual void ShowCharacter(ChatData chatData)
    {
        ShowCharacter(chatData.left, chatData.middle, chatData.right);
    }
    protected virtual void ShowCharacter(CharData left, CharData middle, CharData right)
    {
        ShowCharacter(left, DialogPos.Left);
        ShowCharacter(middle, DialogPos.Middle);
        ShowCharacter(right, DialogPos.Right);
    }
    protected virtual void ShowCharacter(CharData charData,DialogPos pos)
    {
        CharSO charSO = charData.charSO;
        if (charSO == null)
        {
            if (mChars[(int)pos]!=null)
                mChars[(int)pos].gameObject.SetActive(false);
            mChars[(int)pos] = null;
            return;
        }
        if (!mCharChace.ContainsKey(charSO))
        {
            GameObject charObj = BaseCharacter.Instantiate(mCharacter, mCanvas);
            BaseCharacter baseCharacter = charObj.GetComponent<BaseCharacter>();
            baseCharacter.SetData(charSO);
            mCharChace.Add(charSO, baseCharacter);
            baseCharacter.transform.localScale *= charSO.scale;
        }
        SetDialogPos(mCharChace[charSO], pos);
        mCharChace[charSO].SetAction(charData.actionData);
        mChars[(int)pos] = mCharChace[charSO];
        //生成
    }
    public virtual void ShowAction(DialogPos pos, ActionData actionData)
    {
        if (mChars[(int)pos] != null)
        {
            mChars[(int)pos].SetAction(actionData);
        }
    }
    public virtual void ShowDiff(DialogPos pos, DiffTag diffTag)
    {
        if (mChars[(int)pos] != null)
        {
            mChars[(int)pos].SetDiff(diffTag);
        }
    }
    public virtual void SetBackground(Sprite background)
    {
        if (background == null)
            return;
        if (background == mBG.sprite)
            return;
        mMaskBG.gameObject.SetActive(true);
        mMaskBG.sprite = mBG.sprite;
        mBG.sprite = background;
        mMaskBG.color = Color.white;
        mMaskBG.DOColor(Color.clear, 1f).OnComplete(()=>mMaskBG.gameObject.SetActive(false));
    }   
}
