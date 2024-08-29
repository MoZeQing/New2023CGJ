using DG.Tweening;
using Dialog;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStage : BaseStage
{
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
                    character.transform.position = mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset * 0.001f;
                else
                    character.transform.DOMove(mPositions[(int)newChars.IndexOf(character)].transform.position + (Vector3)offset*0.001f, 0.5f);
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
            newChars[i]?.transform.DOMove((mPositions[(int)i].transform.position + (Vector3)offset* 0.001f) , 0.5f);
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
            baseCharacter.transform.position = mPositions[(int)pos].transform.position + offset * 0.001f;
            mCharChace.Add(charSO, baseCharacter);
            baseCharacter.transform.localScale *= charSO.scale;
        }
        //Éú³É
    }
    public override void SetBackground(Sprite sprite)
    {

    }
}
