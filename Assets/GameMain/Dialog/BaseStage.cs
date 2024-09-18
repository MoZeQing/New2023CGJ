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
    protected virtual void SetDialogPos(ChatData chatData)
    {
        List<BaseCharacter> newChars = new List<BaseCharacter>
        {
            chatData.leftAction.charSO!=null?mCharChace[chatData.leftAction.charSO]:null,
            chatData.middleAction.charSO!=null?mCharChace[chatData.middleAction.charSO]:null,
            chatData.rightAction.charSO!=null?mCharChace[chatData.rightAction.charSO]:null
        };
        foreach (BaseCharacter character in mCharChace.Values)
        {
            if (newChars.Contains(character))
            {
                Vector3? offset = character.CharSO.offset;
                if (character.CharacterTag == CharacterTag.Hide)
                    character.transform.position = mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset;
                else
                    character.transform.DOMove(mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset, 0.5f);
                character.Show();
            }
            else
            {
                character.Hide();
            }
        }
        for (int i=0;i< newChars.Count;i++)
        {
            Vector3? offset = newChars[i]?.CharSO.offset;
            newChars[i]?.transform.DOMove(mPositions[(int)i].transform.position + (Vector3)offset, 0.5f);
        }
        mChars= newChars;
    }
    public virtual void ShowCharacter(ChatData chatData)
    {
        ShowCharacter(chatData.leftAction, chatData.middleAction, chatData.rightAction);
        SetDialogPos(chatData);
        SetAction(chatData);
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
    protected virtual void SetAction(ChatData chatData)
    {
        mChars[(int)DialogPos.Left]?.SetAction(chatData.leftAction.actionTag);
        mChars[(int)DialogPos.Left]?.SetDiff(chatData.leftAction.diffTag);
        mChars[(int)DialogPos.Middle]?.SetAction(chatData.middleAction.actionTag);
        mChars[(int)DialogPos.Middle]?.SetDiff(chatData.middleAction.diffTag);
        mChars[(int)DialogPos.Right]?.SetAction(chatData.rightAction.actionTag);
        mChars[(int)DialogPos.Right]?.SetDiff(chatData.rightAction.diffTag);
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
        //生成
    }
}

public enum DialogPos
{
    Left = 0,//左侧
    Middle = 1,//中间
    Right = 2,//右侧
    All=3,
    None=4
}