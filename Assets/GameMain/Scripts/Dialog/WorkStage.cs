using DG.Tweening;
using Dialog;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStage : BaseStage
{
    protected override void SetDialogPos(BaseCharacter baseCharacter, DialogPos dialogPos)
    {
        for (int i = 0; i < mChars.Count; i++)
        {
            if (i == (int)dialogPos)
            {
                if (mChars[i] != baseCharacter && mChars[i] != null)
                    mChars[i].Hide();
                if (mChars[i] == baseCharacter)
                    continue;
                mChars[i] = baseCharacter;
                Vector3 offset = mChars[i].CharSO.offset;
                mChars[i].transform.DOMove(mPositions[(int)dialogPos].transform.position + offset * 0.01f, 0.5f);
            }
            else
            {
                if (baseCharacter == mChars[i])
                    mChars[i] = null;
            }
        }
    }
    protected override void SetDialogPos(ChatData chatData)
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
                    character.transform.position = mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset * 0.01f;
                else
                    character.transform.DOMove(mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset*0.01f, 0.5f);
                character.Show();
            }
            else
            {
                character.Hide();
            }
        }
        for (int i = 0; i < newChars.Count; i++)
        {
            Vector3? offset = newChars[i]?.CharSO.offset;
            newChars[i]?.transform.DOMove((mPositions[(int)i].transform.position + (Vector3)offset* 0.01f) , 0.5f);
        }
        mChars = newChars;
    }
    protected override void ShowCharacter(ActionData actionData, DialogPos pos)
    {
        CharSO charSO = actionData.charSO;
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
            baseCharacter.transform.position = mPositions[(int)pos].transform.position + offset * 0.01f;
            mCharChace.Add(charSO, baseCharacter);
            baseCharacter.transform.localScale *= charSO.scale;
        }
        //Éú³É
    }
}
