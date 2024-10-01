using GameFramework.Event;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using Dialog;

public class TeachStage : BaseStage
{
    [SerializeField] protected BaseCharacter baseCharacter;
    [SerializeField] private CharSO charSO=null;

    public override void ShowCharacter(ChatData chatData)
    {
        if (GameMain.GameEntry.Player.Day == 22)
        {
            baseCharacter.gameObject.SetActive(false);
            return;
        }
        else
        {
            baseCharacter.gameObject.SetActive(true);
        }
        baseCharacter.SetData(charSO);
        baseCharacter.SetAction(chatData.middleAction.actionTag);
        baseCharacter.SetDiff(chatData.middleAction.diffTag);
    }
}
