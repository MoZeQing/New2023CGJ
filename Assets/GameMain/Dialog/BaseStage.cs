using GameMain;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using Dialog;
using DG.Tweening;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

public class BaseStage : MonoBehaviour
{
    [SerializeField] protected Image mBG;
    [SerializeField] protected Transform mCanvas;
    [SerializeField] protected GameObject mCharacter;
    [SerializeField] protected List<Transform> mPositions = new List<Transform>();
    protected List<BaseCharacter> mChars = new List<BaseCharacter>();
    protected Dictionary<CharSO, BaseCharacter> mCharChace = new Dictionary<CharSO, BaseCharacter>();

    private void Awake()
    {
        for (int i = 0; i < mPositions.Count; i++)
        {
            mChars.Add(null);//忽略初始化项目
        }
    }
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
                    mChars[i].Hide();
                if (mChars[i] == baseCharacter)
                    continue;
                mChars[i] = baseCharacter;
                Vector3 offset = mChars[i].CharSO.offset;
                mChars[i].transform.DOMove(mPositions[(int)dialogPos].transform.position + offset, 0.5f);
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
        ShowCharacter(chatData.leftAction, chatData.middleAction, chatData.rightAction);
        HighLightCharacter(chatData.chatPos);
    }
    protected virtual void ShowCharacter(ActionData left, ActionData middle, ActionData right)
    {
        ShowCharacter(left, DialogPos.Left);
        ShowCharacter(middle, DialogPos.Middle);
        ShowCharacter(right, DialogPos.Right);
    }
    protected virtual void HighLightCharacter(DialogPos dialogPos)
    {
        if(dialogPos == DialogPos.None) return;
        for (int i = 0; i < mChars.Count; i++)
        {
            if (i == (int)dialogPos||dialogPos==DialogPos.All)
            {
                mChars[i]?.MainShow();
            }
            else
            {
                mChars[i]?.SubShow();
            }
        }
    }
    protected virtual void ShowCharacter(ActionData actionData, DialogPos pos)
    {
        CharSO charSO=actionData.charSO;
        if (charSO == null)
        {
            if (mChars[(int)pos] != null)
                mChars[(int)pos].Hide();
            mChars[(int)pos] = null;
            return;
        }
        if (!mCharChace.ContainsKey(charSO))
        {
            GameObject charObj = BaseCharacter.Instantiate(mCharacter, mCanvas);
            BaseCharacter baseCharacter = charObj.GetComponent<BaseCharacter>();
            baseCharacter.SetData(charSO);
            Vector3 offset = baseCharacter.CharSO.offset;
            baseCharacter.transform.position = mPositions[(int)pos].transform.position + offset;
            mCharChace.Add(charSO, baseCharacter);
            baseCharacter.transform.localScale *= charSO.scale;
        }
        SetDialogPos(mCharChace[charSO], pos);
        mChars[(int)pos].SetAction(actionData.actionTag);
        mChars[(int)pos].SetDiff(actionData.diffTag);
        mChars[(int)pos] = mCharChace[charSO];
        //生成
    }
    public virtual void SetBackground(Sprite sprite)
    { 
        if(sprite!=null)
            mBG.sprite = sprite;
    }   
}

public enum DialogPos
{
    Left = 0,//左侧
    Middle = 1,//中间
    Right = 2,//两边
    All=3,
    None=4
}